public static class GameSession
{
    public static SongData Data;
    public static int SelectedIndex = -1;

    public static bool HasSelection =>
        Data != null &&
        SelectedIndex >= 0 &&
        Data.songs != null &&
        SelectedIndex < Data.songs.Length;

    public static SongEntry SelectedSong => HasSelection ? Data.songs[SelectedIndex] : null;
}