using System;

namespace Automaton
{
    public sealed class KeyStates
    {
        public static Command[] Idle =
  {
            Command.SETTINGS,
            Command.ADD_NEW_PATIENT
        };

        public static Command[] Positioning =
        {
            Command.SWITCH_EYE,
            Command.CHOOSE_PATIENT_AGE,
            Command.GO_TO_IDLE,
            Command.EXAM_START,
            Command.CR_UP,
            Command.CR_DOWN,
            Command.MOVE_UP,
            Command.MOVE_DOWN,
            Command.MOVE_LEFT,
            Command.MOVE_RIGHT,
            Command.FOCUS_IN,
            Command.FOCUS_OUT,
            Command.FOCUS_HOME,
            Command.SCREEN_CLICK
        };
        public static Command[] Manual =
        {
            Command.CR_UP,
            Command.CR_DOWN,
            Command.MOVE_UP,
            Command.MOVE_DOWN,
            Command.MOVE_LEFT,
            Command.MOVE_RIGHT,
            Command.FOCUS_IN,
            Command.FOCUS_OUT,
            Command.FOCUS_HOME,
            Command.EXAM_SKIP,
            Command.EXAM_AQUIRE,
            Command.SCREEN_CLICK
        };
        public static Command[] PopUp =
        {
            Command.DIALOG_YES,
            Command.DIALOG_NO,
            Command.DIALOG_CANCEL,
            Command.DIALOG_OK
        };
        public static Command[] Measuring =
        {
            Command.EXAM_SKIP,
            Command.EXAM_ABORT,
            Command.WF_GAIN_UP,
            Command.WF_GAIN_DOWN,
            Command.CR_UP,
            Command.CR_DOWN,
            Command.MOVE_UP,
            Command.MOVE_DOWN,
            Command.MOVE_LEFT,
            Command.MOVE_RIGHT,
            Command.SCREEN_CLICK
        };
        public static Command[] Export =
        {
            Command.EXPORD_TO_PDF,
            Command.EXPORD_TO_XML
        };
    }

    public enum Command
    {
        //idle commands
        SETTINGS,                       //send with Settings class 
        ADD_NEW_PATIENT,                //send with PatientInfo class

        //positioning commands
        SWITCH_EYE,                     //send with MeasurementSettings.EyeSide class
        CHOOSE_PATIENT_AGE,             //send with MeasurementSettings.PatientMeaurementType class
        GO_TO_IDLE,                     //send with nothing
        EXAM_START,                     //send with PositioningSettings class
        CR_UP,                          //send with MotorMovementData.amountOfMM
        CR_DOWN,                        //send with MotorMovementData.amountOfMM
        MOVE_UP,                        //send with MotorMovementData.amountOfMM
        MOVE_DOWN,                      //send with MotorMovementData.amountOfMM
        MOVE_LEFT,                      //send with MotorMovementData.amountOfMM
        MOVE_RIGHT,                     //send with MotorMovementData.amountOfMM
        FOCUS_IN,                       //send with MotorMovementData.amountOfMM
        FOCUS_OUT,                      //send with MotorMovementData.amountOfMM
        FOCUS_HOME,                     //send with nothing
        SCREEN_CLICK,                   //send with MotorMovementData.screenClick

        //manual command

        //pop up command
        DIALOG_YES,                     //send with nothing
        DIALOG_NO,                      //send with nothing
        DIALOG_CANCEL,                  //send with nothing
        DIALOG_OK,                      //send with nothing

        //measuring commands
        EXAM_AQUIRE,                    //send with nothing
        EXAM_SKIP,                      //send with nothing
        EXAM_ABORT,                     //send with nothing
        WF_GAIN_UP,                     //send with nothing
        WF_GAIN_DOWN,                   //send with nothing


        //export commands
        EXPORD_TO_PDF,                  //send with nothing
        EXPORD_TO_XML                   //send with nothing
    }

    //Patient Settings
    public class PatientInfo
    {
        //enums
        public enum Gender
        {
            PATIENTS_GENDER_UNKNOWN,
            PATIENTS_GENDER_MAN,
            PATIENTS_GENDER_WOMAN
        };
        public class PatientData
        {
            public string firstName;
            public string LastName;
            public DateTime birthDate;
            public Gender gender;
            public int id;
            public String Email;
        }

        //members
        public PatientData patientData;
        public Gender gender;
    }

    //Measurment Settings
    public class MeasurementSettings
    {
        //enums
        public enum MeasurementType
        {
            MEAS_ALL,
            MEAS_WF,
            MEAS_PACHY,
            MEAS_TOPO,
            MEAS_TONO,
            MEAS_PHOTO,
            MEAS_FUNDUS,
            MEAS_PUPIL,
            MEAS_CATARACT,
            MEAS_DRY_EYE,
            MEAS_GLUCOMA,
            MEAS_GLUCOMA_F,
            MEAS_ANTERIUR_SEGMENT
        }

        public enum EyeSide
        {
            EXAM_BOTH,
            EXAM_LEFT,
            EXAM_RIGHT
        }
        public enum WithNV
        {
            YES,
            NO
        }
        public enum PatientMeaurementType
        {
            CHILD,
            ADULT
        }

        //members
        public PatientMeaurementType meaurementType;
        public WithNV withNV;
        public EyeSide eyeSide;
        public PatientMeaurementType patientType;
        public int numberOfPufs;
        public int readingDistance;

    }

    public class PositioningSettings
    {
        public PatientInfo patientInfo;
        public MeasurementSettings measurementSettings;
    }

    //this class is for use when remote want to position and in positiong state.
    public class MotorMovementData
    {
        //in case that in remote the clicked  on screen, in precentage from center. for exapmle -  (1,1) move one precent from the form x[0,100] y[0,100] 
        public class ScreenClick
        {
            int x;
            int y;
        }
        public int amountOfMM;          //amount of mm...
        public ScreenClick screenClick;
    }

    //this class is the settings class, each time that Tessan want to change the settings they will send the whole structure.
    public class Settings
    {
        public string language;
    }

    //messages that send from VX650 to Tessan, Tessan will need to case the JSON to OutgoingStatus
    public class OutgoingStatus
    {
        //enums
        public enum StatusMessages
        {
            // measurment message
            MEASUREMENT_SIDE_LEFT,
            MEASUREMENT_SIDE_RIGHT,

            //WF measurment message
            WF_FOCUSING_AND_CENTERING,
            WF_MEASURING,
            WF_IMPROVE_SPOT_IMAGE,
            WF_MEASURMENT_DONE,
            WF_MEASURMENT_SKIPPED,

            //Pachy measurment message
            PACHY_FOCUSING_AND_CENTERING,
            PACHY_MEASURING,
            PACHY_MANUAL_FOCUS_CENTER_RINGS_AND_PRESS_ACQURE,
            PACHY_MANUAL_FOCUS_SLIT_AND_PRESS_ACQUIRE,
            PACHY_MEASURMENT_DONE,
            PACHY_MEASURMENT_SKIPPED,

            //Topo measurment message
            TOPO_FOCUSING_AND_CENTERING,
            TOPO_MEASURING,
            TOPO_MANUAL_FOCUS_CENTER_RINGS_AND_PRESS_ACQURE,
            TOPO_MEASURMENT_DONE,
            TOPO_MEASURMENT_SKIPPED,

            //Tono measurment message
            TONO_FOCUSING_AND_CENTERING,
            TONO_MEASURING_PUFF1,
            TONO_MEASURING_PUFF2,
            TONO_MEASURING_PUFF3,
            TONO_MANUAL_MODE,

            //Photo measurment message
            PHOTO_FOCUSING_AND_CENTERING,
            PHOTO_MEASURING,
            PHOTO_MANUAL_PRESS_CAPTURE_OR_FINISH,
            PHOTO_MEASURMENT_DONE,

            //Fundus measurment message
            FUNDUS_FOCUSING_AND_CENTERING,
            FUNDUS_MEASURING,
            FUNDUS_MANUAL_FOCUS_AND_CENTER_AND_PRESS_ACQUIRE,
            FUNDUS_MEASURMENT_DONE,

            //measurment message
            MEASURMENT_SKIPPED,
            MEASURMENT_ABORTED,
            MEASURMENT_COMPLETED,

            //Export message
            RESULTS_READY,
            RESULTS_EXPORTED,

            //Patient management massage
            PATIENT_ADDED,
            PATIENT_DELETED
        }

        //members
        public StatusMessages status;
        public String DisplayMessage; //for example Pop up "please Blink Twice"
    }


}
