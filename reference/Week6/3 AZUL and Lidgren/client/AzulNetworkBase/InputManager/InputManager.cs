using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzulNetworkBase
{
    public enum INPUTAXIS
    {
        HORIZONTAL_P1,
        HORIZONTAL_P2,
        VERTICAL_P1,
        VERTICAL_P2
    }

    public enum INPUTBUTTON
    {
        JUMP,
        FIRE,
        FIRE2
    }
    

    class InputManager
    {
        private static InputManager instance;
        private static InputManager Instance
        {
            get
            {
                if(instance == null)
                {
                    instance = new InputManager();
                }
                return instance;
            }
        }

        KeyState A;
        KeyState D;
        KeyState W;
        KeyState S;
        KeyState UP_ARROW;
        KeyState DOWN_ARROW;
        KeyState LEFT_ARROW;
        KeyState RIGHT_ARROW;
        KeyState SPACE;
        MouseButtonState MOUSE1;
        MouseButtonState MOUSE2;


        private InputManager()
        {

            A = new KeyState(Azul.AZUL_KEY.KEY_A);
            D = new KeyState(Azul.AZUL_KEY.KEY_D);
            W = new KeyState(Azul.AZUL_KEY.KEY_W);
            S = new KeyState(Azul.AZUL_KEY.KEY_S);
            UP_ARROW = new KeyState(Azul.AZUL_KEY.KEY_ARROW_UP);
            DOWN_ARROW = new KeyState(Azul.AZUL_KEY.KEY_ARROW_DOWN);
            LEFT_ARROW = new KeyState(Azul.AZUL_KEY.KEY_ARROW_LEFT);
            RIGHT_ARROW = new KeyState(Azul.AZUL_KEY.KEY_ARROW_RIGHT);
            SPACE = new KeyState(Azul.AZUL_KEY.KEY_SPACE);
            MOUSE1 = new MouseButtonState(Azul.AZUL_MOUSE.BUTTON_1);
            MOUSE2 = new MouseButtonState(Azul.AZUL_MOUSE.BUTTON_2);
        }

        public static void Update()
        {
            Instance.KeyStateUpdate();
           

        }

        //   Function:      GetAxis
        //   Discription:   Gets a value of user input along an Input Axis 
        //   Parameter:     InputAxis name
        //                      What Input are you listening for.
        //                      EX:  Horizontal, will return a value based on all the inputs that are considered 
        //                          horizontal input. i.e. <-, ->, a, d
        //   Return:        int value
        //                      value returned will be either -1, 0, 1. (represents positive, none, negative, input)
        //                      e.x. if 'a' is pressed only then Horizontal will return -1.  
        public static int GetAxis(INPUTAXIS name)
        {
            int output = 0;

            switch(name)
            {
                case INPUTAXIS.HORIZONTAL_P1:
                    output = CalculateAxis(Instance.D, Instance.A);
                    break;
                case INPUTAXIS.HORIZONTAL_P2:
                    output = CalculateAxis(Instance.RIGHT_ARROW, Instance.LEFT_ARROW);
                    break;
                case INPUTAXIS.VERTICAL_P1:
                    output = CalculateAxis(Instance.W, Instance.S);
                    break;
                case INPUTAXIS.VERTICAL_P2:
                    output = CalculateAxis(Instance.UP_ARROW, Instance.DOWN_ARROW);
                    break;

            }
            
            return output;
        }

        public static bool GetButton(INPUTBUTTON name)
        {
            bool output = false;

            switch (name)
            {
                case INPUTBUTTON.FIRE:
                    output = CalculateButton(Instance.MOUSE1);
                    break;
                case INPUTBUTTON.FIRE2:
                    output = CalculateButton(Instance.MOUSE2);
                    break;
                case INPUTBUTTON.JUMP:
                    output = CalculateButton(Instance.SPACE);
                    break;
            }

            return output;
        }

        public static bool GetButtonDown(INPUTBUTTON name)
        {
            bool output = false;

            switch (name)
            {
                case INPUTBUTTON.FIRE:
                    output = CalculateButtonDown(Instance.MOUSE1);
                    break;
                case INPUTBUTTON.FIRE2:
                    output = CalculateButtonDown(Instance.MOUSE2);
                    break;
                case INPUTBUTTON.JUMP:
                    output = CalculateButtonDown(Instance.SPACE);
                    break;
            }

            return output;
        }

        public static bool GetButtonUp(INPUTBUTTON name)
        {
            bool output = false;

            switch (name)
            {
                case INPUTBUTTON.FIRE:
                    output = CalculateButtonUp(Instance.MOUSE1);
                    break;
                case INPUTBUTTON.FIRE2:
                    output = CalculateButtonUp(Instance.MOUSE2);
                    break;
                case INPUTBUTTON.JUMP:
                    output = CalculateButtonUp(Instance.SPACE);
                    break;
            }

            return output;
        }


        private void KeyStateUpdate()
        {
            A.Update();
            W.Update();
            S.Update();
            D.Update();
            UP_ARROW.Update();
            DOWN_ARROW.Update();
            RIGHT_ARROW.Update();
            LEFT_ARROW.Update();
            SPACE.Update();
            MOUSE1.Update();
            MOUSE2.Update();
        }
        

        private static bool CalculateButton(MouseButtonState key)
        {
            return key.Pressed();
        }

        private static bool CalculateButton(KeyState key)
        {
            return key.Pressed();
        }

        private static bool CalculateButtonDown(MouseButtonState key)
        {
            return key.PressedDown();
        }
        private static bool CalculateButtonDown(KeyState key)
        {
            return key.PressedDown();
        }

        private static bool CalculateButtonUp(MouseButtonState key)
        {
            return key.PressedUp();
        }

        private static bool CalculateButtonUp(KeyState key)
        {
            return key.PressedUp();
        }

        private static int CalculateAxis(KeyState positiveKey, KeyState negativeKey)
        {
            return (positiveKey.Pressed() ? 1:0) - (negativeKey.Pressed()?1:0);
        }


    }

    


}
