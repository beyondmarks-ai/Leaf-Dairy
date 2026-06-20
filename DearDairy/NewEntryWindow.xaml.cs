using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace DearDairy;

public partial class NewEntryWindow : UserControl
{
    private readonly DispatcherTimer _waveTimer;
    private readonly Random _random = new();
    private DateTime _recordingStarted;
    private bool _isRecording;

    public event EventHandler? CloseRequested;

    public NewEntryWindow()
    {
        InitializeComponent();
        _waveTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(120) };
        _waveTimer.Tick += AnimateWaveform;
    }

    private void MicButton_Click(object sender, RoutedEventArgs e)
    {
        _isRecording = !_isRecording;
        if (_isRecording)
        {
            _recordingStarted = DateTime.Now;
            MicStatus.Text = "Listening... tap again to finish";
            MicStatus.Foreground = new SolidColorBrush(Color.FromRgb(255, 196, 94));
            MicButton.Background = new SolidColorBrush(Color.FromRgb(85, 45, 17));
            _waveTimer.Start();
            return;
        }

        _waveTimer.Stop();
        var elapsed = DateTime.Now - _recordingStarted;
        MicStatus.Text = $"Voice note ready  •  {elapsed:mm\\:ss}";
        MicButton.Background = new SolidColorBrush(Color.FromRgb(26, 21, 16));
        ResetWaveform();
    }

    private void AnimateWaveform(object? sender, EventArgs e)
    {
        foreach (var bar in WaveBars())
            bar.Height = _random.Next(10, 39);
    }

    private IEnumerable<Rectangle> WaveBars() =>
        [Wave1, Wave2, Wave3, Wave4, Wave5, Wave6, Wave7, Wave8, Wave9, Wave10];

    private void ResetWaveform()
    {
        var heights = new[] { 12d, 24, 34, 18, 28, 28, 18, 34, 24, 12 };
        var bars = WaveBars().ToArray();
        for (var i = 0; i < bars.Length; i++) bars[i].Height = heights[i];
    }

    private void WriteWithAI_Click(object sender, RoutedEventArgs e)
    {
        EditorPanel.Visibility = Visibility.Visible;
        EntryEditor.Focus();
        EntryEditor.SelectAll();
    }

    private void QuickAction_Click(object sender, RoutedEventArgs e)
    {
        EditorPanel.Visibility = Visibility.Visible;
        EntryEditor.Text = $"New {(sender as Button)?.Content?.ToString()?.ToLowerInvariant()}...";
        EntryEditor.Focus();
        EntryEditor.CaretIndex = EntryEditor.Text.Length;
    }

    private void Save_Click(object sender, RoutedEventArgs e)
    {
        CloseRequested?.Invoke(this, EventArgs.Empty);
    }

    private void Close_Click(object sender, RoutedEventArgs e) =>
        CloseRequested?.Invoke(this, EventArgs.Empty);

    protected override void OnVisualParentChanged(DependencyObject oldParent)
    {
        if (VisualParent is null) _waveTimer.Stop();
        base.OnVisualParentChanged(oldParent);
    }
}
