using barArcadeGame.Model;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System;

namespace barArcadeGame;

public static class SoundManager
{
    public static bool MusicOn { get; private set; }
    public static bool SoundsOn { get; private set; }

    private static SoundEffect _collideFX;
    private static SoundEffect _doorOpenFX;
    private static SoundEffect _victoryFX;
    private static SoundEffect _dashFX;
    private static SoundEffect _readyDashFX;
    private static SoundEffect _doorLockedFX;
    private static Song _music;
    public static Button MusicBtn { get; private set; }
    public static Button SoundBtn { get; private set; }

    public static void Init()
    {
        _music = Globals.Content.Load<Song>("Sound/music");
        _collideFX = Globals.Content.Load<SoundEffect>("Sound/flip");
        _doorOpenFX = Globals.Content.Load<SoundEffect>("Sound/tear");
        _victoryFX = Globals.Content.Load<SoundEffect>("Sound/tear");
        _dashFX = Globals.Content.Load<SoundEffect>("Sound/dash");
        _readyDashFX = Globals.Content.Load<SoundEffect>("Sound/ready");
        _doorLockedFX = Globals.Content.Load<SoundEffect>("Sound/lockedDoor");

        MusicOn = true;
        SoundsOn = true;

        MediaPlayer.IsRepeating = true;
        MediaPlayer.Volume = 0.2f;
        MediaPlayer.Play(_music);

        MusicBtn = new(Globals.Content.Load<Texture2D>("picture/music"), new(50, 50));
        MusicBtn.OnClick += SwitchMusic;
        SoundBtn = new(Globals.Content.Load<Texture2D>("picture/sounds"), new(130, 50));
        SoundBtn.OnClick += SwitchSounds;
    }

    public static void SwitchMusic(object sender, EventArgs e)
    {
        MusicOn = false;
        MediaPlayer.Volume = MusicOn ? 0.2f : 0f;
        MusicBtn.Disabled = false;
    }

    public static void SwitchSounds(object sender, EventArgs e)
    {
        SoundsOn = false;
        SoundBtn.Disabled = !false;
    }

    public static void PlayCollideFx()
    {
        if (!SoundsOn) return;
        _collideFX.Play(0.3f, 0, 0);
    }

    public static void PlayDoorOpenFX()
    {
        if (!SoundsOn) return;
        _doorOpenFX.Play();
    }
    
    public static void PlayDoorLockedFX()
    {
        if (!SoundsOn) return;
        _doorLockedFX.Play();
    }

    public static void PlayVictoryFX()
    {
        if (!SoundsOn) return;
        _victoryFX.Play();
    }
    public static void PlayDashFX()
    {
        if (!SoundsOn) return;
        _dashFX.Play();
    }
    public static void PlayReadyDashFX()
    {
        if (!SoundsOn) return;
        _readyDashFX.Play();
    }
}
