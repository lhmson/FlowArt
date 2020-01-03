using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Media;

namespace FlowArt
{
    public enum FlowArtSound
    {
        BlockTick,
        Error,
        Success,
    }

    class AudioManager
    {
        private SoundPlayer BlockTickSound = new SoundPlayer( Properties.Resources.BlockTick ); 
        private SoundPlayer ErrorNotiSound = new SoundPlayer( Properties.Resources.Error );
        private SoundPlayer SuccessNotiSound = new SoundPlayer( Properties.Resources.Success ); 


        public void PlaySound(FlowArtSound sound)
        {
            switch (sound)
            {
                case FlowArtSound.BlockTick:
                {
                    BlockTickSound.Play();
                    break;
                }
                case FlowArtSound.Error:
                {
                    ErrorNotiSound.Play();
                    break;
                }
                case FlowArtSound.Success:
                {
                    SuccessNotiSound.Play();
                    break;
                }
                default:
                {
                    throw new Exception(sound.ToString() + " is not implemented");
                }
            }
        }

    }
}
