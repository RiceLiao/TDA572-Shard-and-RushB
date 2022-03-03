/*
*
*   @author Haowei Liao
*   @version 1.0
*   
*/

using System;
using SDL2;

namespace Shard
{
    class SoundSDL : Sound
    {
        SDL.SDL_AudioSpec have, want;
        uint length, musicdDev, soundDev;
        IntPtr buffer;
        bool musicPlaying;

        public override void pauseMusic()
        {
            if (musicPlaying)
            {
                SDL.SDL_PauseAudioDevice(musicdDev, 1);
                musicPlaying = false;
            }
            else
            {
                SDL.SDL_PauseAudioDevice(musicdDev, 0);
                musicPlaying = true;
            }
        }

        public override void playMusic(string soundPath)
        {
            SDL.SDL_LoadWAV(soundPath, out have, out buffer, out length);
            musicdDev = SDL.SDL_OpenAudioDevice(IntPtr.Zero, 0, ref have, out want, 0);
            SDL.SDL_QueueAudio(musicdDev, buffer, length);
            SDL.SDL_PauseAudioDevice(musicdDev, 0);
            musicPlaying = true;
        }

        public override void playSound(string soundPath)
        {
            SDL.SDL_CloseAudioDevice(soundDev);
            SDL.SDL_LoadWAV(soundPath, out have, out buffer, out length);
            soundDev = SDL.SDL_OpenAudioDevice(IntPtr.Zero, 0, ref have, out want, 0);
            SDL.SDL_QueueAudio(soundDev, buffer, length);
            SDL.SDL_PauseAudioDevice(soundDev, 0);
        }
    }
}
