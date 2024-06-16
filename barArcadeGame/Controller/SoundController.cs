using barArcadeGame.Model;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System;
using System.Diagnostics;

namespace barArcadeGame;

public static class SoundController
{
    public static bool MusicOn { get; private set; }
    public static bool SoundsOn { get; private set; }

    private static SoundEffect collideFX;
    private static SoundEffect doorOpenFX;
    private static SoundEffect victoryFX;
    private static SoundEffect dashFX;
    private static SoundEffect readyDashFX;
    private static SoundEffect doorLockedFX;
    private static Song music;
    private static SoundEffect fightmusic;
    //private static SoundEffect hurtFX;
    private static SoundEffectInstance fightFXInstance;

    public static Button MusicBtn { get; private set; }
    public static Button SoundBtn { get; private set; }

    public static void Init()
    {
        collideFX = Globals.Content.Load<SoundEffect>("Sound/flip");
        doorOpenFX = Globals.Content.Load<SoundEffect>("Sound/tear");
        victoryFX = Globals.Content.Load<SoundEffect>("Sound/tear");
        dashFX = Globals.Content.Load<SoundEffect>("Sound/dash");
        readyDashFX = Globals.Content.Load<SoundEffect>("Sound/ready");
        doorLockedFX = Globals.Content.Load<SoundEffect>("Sound/lockedDoor");
        fightmusic = Globals.Content.Load<SoundEffect>("Sound/fight");
       // hurtFX = Globals.Content.Load<SoundEffect>("Sound/hurt");

        fightFXInstance = fightmusic.CreateInstance();

        MusicOn = true;
        SoundsOn = true;


        MusicBtn = new(Globals.Content.Load<Texture2D>("picture/music"), new(50, 50));
        MusicBtn.OnClick += SwitchMusic;
        SoundBtn = new(Globals.Content.Load<Texture2D>("picture/sounds"), new(130, 50));
        SoundBtn.OnClick += SwitchSounds;
    }

    public static void SwitchMusic(object sender, EventArgs e)
    {
        MusicOn = !MusicOn;
        MediaPlayer.Volume = MusicOn ? 0.2f : 0f;
        MusicBtn.Disabled = !MusicOn;
    }

    public static void SwitchSounds(object sender, EventArgs e)
    {
        SoundsOn = !SoundsOn;
        SoundBtn.Disabled = !SoundsOn;
    }

    public static void PlayCollideFx()
    {
        if (!SoundsOn) return;
        collideFX.Play(0.3f, 0, 0);
    }

    public static void PlayHurtFx()
    {
       /* if (!SoundsOn) return;
        hurtFX.Play(0.3f, 0, 0);*/
    }

  
    public static void PlayJazzFx()
    {
            music = Globals.Content.Load<Song>("Sound/music");
            
        MediaPlayer.IsRepeating = true;
        MediaPlayer.Play(music);
        MediaPlayer.Volume = 0.2f;

    }

    public static void PlayBattleFx()
    {

        MediaPlayer.Stop();
        fightFXInstance.IsLooped = true;
        fightmusic.Play();
    }

    public static void PlayDoorOpenFX()
    {
        if (!SoundsOn) return;
        doorOpenFX.Play();
    }

    public static void PlayDoorLockedFX()
    {
        if (!SoundsOn) return;
        doorLockedFX.Play();
    }

    public static void PlayVictoryFX()
    {
        if (!SoundsOn) return;
        victoryFX.Play();
    }

    public static void PlayDashFX()
    {
        if (!SoundsOn) return;
        dashFX.Play();
    }

    public static void PlayReadyDashFX()
    {
        if (!SoundsOn) return;
        readyDashFX.Play();
    }
}
