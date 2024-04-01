﻿using Molten.Audio;
using Molten.Collections;
using Molten.Graphics;
using Molten.Input;
using Molten.Threading;
using Molten.UI;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Reflection;

namespace Molten.Examples;

public class ExampleBrowser<R, I, A> : Foundation
    where R : RenderService, new()
    where I : InputService, new ()
    where A : AudioService, new()
{
    class ExampleBindings
    {
        public UIWindow Window;

        public INativeSurface NativeWindow;
    }

    bool _baseContentLoaded;
    Scene _scene;
    CameraComponent _cam2D;
    ContentLoadBatch _loader;
    SpriteFont _font;

    UILabel _txtDebug;
    UILabel _txtMovement;
    UILabel _txtGamepad;
    UIListView _lstExamples;
    UIButton _btnCloseAll;
    UIButton _btnStart;
    UICheckBox _chkNativeWindow;

    Sprite _windowIcon;

    ConcurrentDictionary<MoltenExample, ExampleBindings> _exampleBindings;
    ThreadedList<MoltenExample> _examples;

    public ExampleBrowser(string title) : base(title)
    {
        _exampleBindings = new ConcurrentDictionary<MoltenExample, ExampleBindings>();
        _examples = new ThreadedList<MoltenExample>();
    }

    protected override void OnStart(EngineSettings settings)
    {
        settings.AddService<R>(ThreadingMode.SeparateThread, "Renderer");
        settings.AddService<I>(ThreadingMode.MainThread, "Input");
        settings.AddService<A>(ThreadingMode.SeparateThread, "Audio");
    }

    protected override void OnInitialize(Engine engine)
    {
        base.OnInitialize(engine);

        if (Window != null)
            Window.OnResize += Window_OnResize;

        _scene = new Scene("ExampleBrowser", Engine);
        SpriteLayer = _scene.AddLayer("sprite", true);
        UILayer = _scene.AddLayer("ui", true);
        UILayer.BringToFront();
        UI = UILayer.AddObjectWithComponent<UIManagerComponent>();

        // Use the same camera for both the sprite and UI scenes.
        _cam2D = _scene.AddObjectWithComponent<CameraComponent>(UILayer);
        _cam2D.Mode = RenderCameraMode.Orthographic;
        _cam2D.OrderDepth = 1;
        _cam2D.MaxDrawDistance = 1.0f;
        _cam2D.Surface = Window;
        _cam2D.LayerMask = SceneLayerMask.Layer0;
        _cam2D.BackgroundColor = new Color(0x333333);
        _cam2D.Focus();

        UI.Root.IsScrollingEnabled = false;
        _cam2D.OnSurfaceChanged += UpdateUIRootBounds;
        _cam2D.OnSurfaceResized += UpdateUIRootBounds;

        Settings.Input.PointerSensitivity.Value = 0.75f;
        Settings.Input.PointerSensitivity.Apply();

        if (engine.Input != null && engine.Input.State == EngineServiceState.Ready)
        {
            Engine.Input.Surface = Window;
            HookWindow(Window);
        }

        _loader = Engine.Content.GetLoadBatch();
        _loader.LoadFont("assets/FiraSans-Bold.ttf", (font, isReload, handle) =>
        {
            _font = font;
            Engine.Renderer.Overlay.Font = _font;
        });

        _loader.Deserialize<UITheme>("assets/test_theme.json", (theme, isReload, handle) => UI.Root.Theme = theme);

        _loader.Load<ITexture2D>("assets/logo_64.png", (tex, isReload, handle) =>
        {
            SpriteData sd = new SpriteData(tex);
            _windowIcon = new Sprite();
            _windowIcon.Data = sd;
            _windowIcon.Origin = new Vector2F(1f);
        });

        _loader.OnCompleted += OnBaseContentLoaded;
        _loader.Dispatch();
    }

    private void HookWindow(INativeSurface surface)
    {
        surface.OnFocusGained += Window_OnFocusGained;
        surface.OnClose += UnhookWindow;
    }

    private void UnhookWindow(INativeSurface surface)
    {
        surface.OnFocusGained -= Window_OnFocusGained;
        surface.OnClose -= UnhookWindow;
    }

    private void Window_OnFocusGained(INativeSurface surface)
    {
        Engine.Input.Surface = surface;
    }

    private void UpdateUIRootBounds(CameraComponent camera, IRenderSurface2D surface)
    {
        UI.Root.LocalBounds = new Rectangle(0, 0, (int)surface.Width, (int)surface.Height);
    }

    private void OnBaseContentLoaded(ContentLoadBatch content)
    {
        SampleSpriteRenderComponent com = UILayer.AddObjectWithComponent<SampleSpriteRenderComponent>();
        com.RenderCallback = OnDrawSprites;
        BuildUI(UI);
        DetectExamples();
        _baseContentLoaded = true; 
        UpdateUIRootBounds(_cam2D, _cam2D.Surface);
    }

    private void DetectExamples()
    {
        List<Type> eTypes = ReflectionHelper.FindType<MoltenExample>();

        // Sort examples alphabetically, by title.
        eTypes.Sort((a, b) =>
        {
            ExampleAttribute attA = a.GetCustomAttribute<ExampleAttribute>();
            ExampleAttribute attB = b.GetCustomAttribute<ExampleAttribute>();
            return StringComparer.CurrentCulture.Compare(attA.Title, attB.Title);
        });

        // Add examples to the browser list.
        foreach(Type t in eTypes)
        {
            ExampleAttribute att = t.GetCustomAttribute<ExampleAttribute>();
            UIExampleListItem item = _lstExamples.Children.Add<UIExampleListItem>(new Rectangle(0, 0, 10, 25));
            item.ExampleType = t;
            item.DoublePressed += BtnStart_StartExample;

            if (att != null)
            {
                item.Text = att.Title;
                item.Description = att.Description;
            }
            else
            {
                item.Text = t.Name;
                item.Description = "[No Description]";
            }
        }
    }

    private void BuildUI(UIManagerComponent ui)
    {
        _txtDebug = new UILabel()
        {
            Text = "[F1] debug overlay",
            HorizontalAlign = UIHorizonalAlignment.Center,
        };

        _txtMovement = new UILabel()
        {
            Text = "[W][A][S][D] to move -- [ESC] close -- [LMB] and [MOUSE] to rotate",
            HorizontalAlign = UIHorizonalAlignment.Center,
        };

        _txtGamepad = new UILabel()
        {
            HorizontalAlign = UIHorizonalAlignment.Center,
        };

        UIPanel cp = UI.Children.Add<UIPanel>(new Rectangle(0, 0, 300, 900));
        UILabel lblExamples = UI.Children.Add<UILabel>(new Rectangle(5, 5, 300, 20));
        lblExamples.Text = "Available examples:";

        _lstExamples = cp.Children.Add<UIListView>(new Rectangle(5, 30, 285, 600));
        _lstExamples.SelectionChanged += _lstExamples_SelectionChanged;

        _btnCloseAll = UI.Children.Add<UIButton>(new Rectangle(25, 650, 130, 25));
        _btnCloseAll.Text = "Close All";
        _btnCloseAll.IsEnabled = false;
        _btnCloseAll.Released += _btnCloseAll_Released;

        _btnStart = UI.Children.Add<UIButton>(new Rectangle(165, 650, 100, 25));
        _btnStart.Text = "Start";
        _btnStart.IsEnabled = false;
        _btnStart.Released += BtnStart_StartExample;

        _chkNativeWindow = UI.Children.Add<UICheckBox>(new Rectangle(5, 690, 200, 25));
        _chkNativeWindow.Text = "Open in Native Window";

        ui.Children.Add(_txtDebug);
        ui.Children.Add(_txtMovement);
        ui.Children.Add(_txtGamepad);

        UpdateGamepadUI();
        Gamepad.OnConnectionStatusChanged += Gamepad_OnConnectionStatusChanged;

        UpdateUIlayout(ui);
    }

    private void _btnCloseAll_Released(UIElement element, CameraInputTracker tracker)
    {
        List<ExampleBindings> bindings = _exampleBindings.Values.ToList();
        foreach(ExampleBindings b in bindings)
        {
            b.Window?.Close();
            b.NativeWindow?.Close();
        }

        _btnCloseAll.IsEnabled = false;
    }

    private void BtnStart_StartExample(UIElement element, CameraInputTracker tracker)
    {
        if (_lstExamples.SelectedItem == null)
            return;

        UIExampleListItem selected = _lstExamples.SelectedItem as UIExampleListItem;
        MoltenExample example = Activator.CreateInstance(selected.ExampleType) as MoltenExample;
        ExampleBindings binding = new ExampleBindings();

        if (_chkNativeWindow.IsChecked)
        {
            binding.NativeWindow = Engine.Renderer.Device.Resources.CreateFormSurface(selected.Text, selected.Text.Replace(" ", ""), 800, 600);

            HookWindow(binding.NativeWindow);

            binding.NativeWindow.OnClose += (nativeSurface) =>
            {
                example.Close();
                if (_exampleBindings.TryRemove(example, out ExampleBindings binding))
                {
                    _examples.Remove(example);
                    example.MainScene.Dispose();
                }
            };

            example.Initialize(this, _font, binding.NativeWindow, binding.NativeWindow, Log);

            binding.NativeWindow.Mode = WindowMode.Windowed;
            binding.NativeWindow.IsVisible = true;
        }
        else
        {
            IRenderSurface2D surface = Engine.Renderer.Device.Resources.CreateSurface(800, 600, name: $"{selected.Text} Example");

            binding.Window = UI.Children.Add<UIWindow>(new Rectangle(400 + Rng.Next(10, 50), 100, 800, 620));
            {
                example.Initialize(this, _font, binding.Window, surface, Log);

                binding.Window.Title = selected.Text;
                binding.Window.Icon = _windowIcon;

                UITexture windowTex = binding.Window.Children.Add<UITexture>(new Rectangle(0, 0, 800, 600));
                windowTex.Focused += (e) => 
                example.IsFocused = true;
                windowTex.Unfocused += (e) => example.IsFocused = false;
                windowTex.Texture = surface;

                binding.Window.Closing += (element, args) =>
                {
                    example.Close();
                    if (_exampleBindings.TryRemove(example, out ExampleBindings binding))
                    {
                        _examples.Remove(example);
                        example.MainScene.Dispose();
                    }
                };

                binding.Window.Minimized += (element) =>
                {
                    example.MainScene.IsVisible = false;
                    example.MainScene.IsEnabled = false;
                };

                binding.Window.Opened += (element) =>
                {
                    example.MainScene.IsVisible = true;
                    example.MainScene.IsEnabled = true;
                };

                binding.Window.Resized += (element) =>
                {
                    int w = binding.Window.RenderBounds.Width;
                    int h = binding.Window.RenderBounds.Height;

                    surface.Resize(GpuPriority.StartOfFrame, (uint)w, (uint)h);
                    windowTex.LocalBounds = new Rectangle(0,0, w, h);
                };
            }
        }

        _exampleBindings.TryAdd(example, binding);
        _examples.Add(example);
        _btnCloseAll.IsEnabled = true;
    }

    private void _lstExamples_SelectionChanged(UIListViewItem element)
    {
        _btnStart.IsEnabled = element != null;
    }

    private void Gamepad_OnConnectionStatusChanged(InputDevice device, bool isConnected)
    {
        UpdateGamepadUI();
    }

    private void Window_OnResize(ISwapChainSurface surface)
    {
        if (_baseContentLoaded)
            UpdateUIlayout(UI);
    }

    private void UpdateGamepadUI()
    {
        if (Gamepad.IsConnected)
        {
            _txtGamepad.Text = "Gamepad [LEFT STICK] or [D-PAD] to move -- [RIGHT STICK] to aim";
        }
        else
        {
            _txtGamepad.Text = "Connect a gamepad / controller";
        }
    }

    protected virtual void UpdateUIlayout(UIManagerComponent ui)
    {
        int xCenter = (int)(Window.Width / 2);
        _txtDebug.LocalBounds = new Rectangle(xCenter, 5, 0, 0);
        _txtGamepad.LocalBounds = new Rectangle(xCenter, (int)Window.Height - 20, 0, 0);
        _txtMovement.LocalBounds = new Rectangle(xCenter, _txtGamepad.LocalBounds.Y - 20, 0, 0);
    }

    /// <summary>
    /// Called when the <see cref="SampleBrowser"/> should update and handle gamepad input. <see cref="SampleBrowser"/> provides default handling.
    /// </summary> 
    /// <param name="time"></param>
    private void OnGamepadInput(Timing time)
    {
        // Apply left and right vibration equal to left and right trigger values 
        Gamepad.VibrationLeft.Value = Gamepad.LeftTrigger.Value;
        Gamepad.VibrationRight.Value = Gamepad.RightTrigger.Value;
    }

    protected override void OnUpdate(Timing time)
    {
        // Don't update until the base content is loaded.
        if (!_baseContentLoaded)
            return;

        // Cycle through window modes.
        if (Engine.Renderer == null || Engine.Renderer.State != EngineServiceState.Running)
            return;

        if (Engine.Input != null && Engine.Input.State == EngineServiceState.Running)
        {
            if (Keyboard.IsTapped(KeyCode.F2))
            {
                switch (Window.Mode)
                {
                    case WindowMode.Borderless: Window.Mode = WindowMode.Windowed; break;
                    case WindowMode.Windowed: Window.Mode = WindowMode.Borderless; break;
                }
            }

            // Toggle overlay.
            if (Keyboard.IsTapped(KeyCode.F1))
            {
                if (_cam2D.HasFlags(RenderCameraFlags.ShowOverlay))
                {
                    int cur = Engine.Renderer.Overlay.Current;

                    // Remove overlay flag if we've hit the last overlay.
                    if (!Engine.Renderer.Overlay.Next())
                    {
                        _cam2D.Flags &= ~RenderCameraFlags.ShowOverlay;
                        Engine.Renderer.Overlay.Current = 0;
                    }
                }
                else
                {
                    Engine.Renderer.Overlay.Current = 0;
                    _cam2D.Flags |= RenderCameraFlags.ShowOverlay;
                }
                Debug.WriteLine($"Overlay toggled with F1 -- Frame ID: {time.FrameID}");
            }
        }

        if (Keyboard.IsTapped(KeyCode.Escape))
            Exit();

        OnGamepadInput(time);

        _examples.For(0, (index, example) => example.Update(Time));
    }

    protected virtual void OnDrawSprites(SpriteBatcher sb)
    {
        if (_font == null)
            return;

        /*string text = $"Focused UI Element: {(UI.FocusedElement != null ? UI.FocusedElement.Name : "None")}";
        Vector2F tSize = _font.MeasureString(text);
        Vector2F pos = new Vector2F()
        {
            X = (Window.Width / 2) - (tSize.X / 2),
            Y = 25,
        };

        sb.DrawString(_font, text, pos, Color.White);*/
    }

    /// <summary>Gets a random number generator. Used for various samples.</summary>
    public Random Rng { get; private set; } = new Random();

    /// <summary>
    /// Gets the sample's UI scene layer.
    /// </summary>
    public SceneLayer UILayer { get; private set; }

    /// <summary>
    /// Gets the sample's sprite scene layer.
    /// </summary>
    public SceneLayer SpriteLayer { get; private set; }

    public UIManagerComponent UI { get; private set; }

    protected Mesh TestMesh { get; private set; }
}
