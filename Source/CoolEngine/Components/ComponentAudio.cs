using System.Collections.Generic;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using OpenGL_Game.Components;
using OpenGL_Game.Systems;
using OpenGL_Game.Managers;
using OpenGL_Game.Objects;
using System.ComponentModel;
using CoolEngine.Xaml;


namespace OpenGL_Game.Components
{
    public class ComponentAudio : IComponent
    {
        Vector3 vpos;
        bool isLoop;
        public static AudioEmitter emitter;
        AudioListener listener;
        SoundEffectInstance soundEffectInstance;

        public ComponentAudio()
        { }

        public ComponentAudio(string audioName,bool loop)
        {
            IsLoop = loop;
            Sound = ResourceManager.LoadSoundEffectInstance(audioName);
        }

        [TypeConverter(typeof(SoundEffectInstanceResourceConverter))]
        public SoundEffectInstance Sound
        {
            get { return soundEffectInstance; }
            set
            {
                if (soundEffectInstance == value)
                {
                    return;
                }
                if (soundEffectInstance != null)
                {
                    soundEffectInstance.Stop();
                }
                soundEffectInstance = value;
                initSound();
            }
        }

        public bool IsLoop
        {
            get { return isLoop; }
            set
            {
                isLoop = value;
                updateSoundLoop();
            }
        }

        private void initSound()
        {
            vpos = Vector3.Zero;
            emitter = new AudioEmitter();
            listener = new AudioListener();
            emitter.Position = vpos;
            soundEffectInstance.Apply3D(listener, emitter);
            updateSoundLoop();
        }

        private void updateSoundLoop()
        {
            if (soundEffectInstance != null)
            {
                if (isLoop)
                {
                    soundEffectInstance.IsLooped = true;
                    soundEffectInstance.Play();
                }
                else
                {
                    soundEffectInstance.IsLooped = false;
                }
            }
        }

        public void changeSound(string audioName)
        {
            Sound = ResourceManager.LoadSoundEffectInstance(audioName);
        }

        public bool stopped()
        {
            return (soundEffectInstance.State == SoundState.Stopped);
        }

        public void setPlay()
        {
            soundEffectInstance.Play();
        }

        public void updateA()
        {
            soundEffectInstance.Apply3D(listener, emitter);

        }

        public Vector3 setPos
        {
            set { emitter.Position = value; }
        }

        public Vector3 Gpos
        {
            set { vpos = value; }
            get { return vpos; }
        }

        public Vector3 Apos
        {
            get { return emitter.Position; }
        }


        public ComponentTypes ComponentType
        {
            get { return ComponentTypes.COMPONENT_AUDIO; }
        }
    }
}