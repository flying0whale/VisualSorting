using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using VisualSorting.Sorting;

namespace VisualSorting;

public partial class MainWindow : Window
{
    private const int LIST_SIZE = 150;
    private const int DURATION = 50;

    private Size _canvasSize;
    private bool _canInput;

    private readonly static List<Border> _pillars = [];

    private static List<int> _list = null!;

    private static readonly SolidColorBrush _pillarColor = (SolidColorBrush)new BrushConverter().ConvertFrom("#1492d9")!;

    public MainWindow ()
    {
        InitializeComponent();
    }

    protected override void OnRender (DrawingContext drawingContext)
    {
        _canvasSize = canvas.RenderSize;
        _list = Render();
    }

    private List<int> GenerateList()
    {
        List<int> list = [];
        for (int i = 1; i < 1 + LIST_SIZE; i++) {
            list.Add(i * 2);
        }

        for (int i = 0; i < LIST_SIZE; i++) {
            for (int j = 0; j < LIST_SIZE; j++) {
                SwapAction.Swap(list, Random.Shared.Next(0, LIST_SIZE), Random.Shared.Next(0, LIST_SIZE));
            }
        }

        return list;
    }

    protected override void OnKeyDown (KeyEventArgs e)
    {
        if (e.Key == Key.R) {
            _list = Render();
        }

        if (!_canInput) { return; }

        if (e.Key == Key.Enter) {
            var actions = _list.Sort(Algorithm.Quicksort);

            EventLoop.Run(async () => await ExecuteQueue(actions));
        }
    }

    private async Task ExecuteQueue (Queue<SwapAction> actions)
    {
        if (actions.Count == 0)
            return;

        var action = actions.Dequeue();
        SwapPillars(action.First, action.Second);

        await Task.Delay(DURATION + 10);

        await ExecuteQueue(actions);
    }

    private void SwapPillars (int first, int second)
    {
        var sb = new Storyboard();

        var pillar1 = _pillars[first];
        var pillar2 = _pillars[second];

        SwapAction.Swap(_pillars, first, second);

        var pos1 = Canvas.GetLeft(pillar1);
        var pos2 = Canvas.GetLeft(pillar2);

        var anim1 = CreatePillarMovement(pillar1, pos1, pos2);
        var anim2 = CreatePillarMovement(pillar2, pos2, pos1);

        sb.Children.Add(anim1);
        sb.Children.Add(anim2);
        sb.Begin();
    }

    private List<int> Render ()
    {
        _canInput = true;

        canvas.Children.Clear();
        _pillars.Clear();

        var width = _canvasSize.Width / LIST_SIZE;
        var space = width / 5;
        width -= space;

        List<int> list = GenerateList();
        for (int i = 0; i < LIST_SIZE; i++) {
            var num = list[i];
            var pillar = CreatePillar(num, width);

            Canvas.SetLeft(pillar, space / 2 + (width + space) * i);
            Canvas.SetTop(pillar, _canvasSize.Height - num);
            canvas.Children.Add(pillar);

            _pillars.Add(pillar);
        }

        return list;
    }

    private DoubleAnimation CreatePillarMovement (Border pillar, double from, double to)
    {
        pillar.Background = Brushes.OrangeRed;

        var anim = new DoubleAnimation {
            From = from,
            To = to,
            Duration = TimeSpan.FromMilliseconds(DURATION)
        };

        Storyboard.SetTarget(anim, pillar);
        Storyboard.SetTargetProperty(anim, new("(Canvas.Left)"));

        anim.Completed += (_, _) => {
            pillar.Background = _pillarColor;
        };

        return anim;
    }

    private Border CreatePillar (int number, double width)
    {
        var border = new Border {
            Background = _pillarColor,
            Width = width,
            Height = number
        };

        return border;
    }
}