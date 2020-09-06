namespace Automaton
{


    //states and commands

    //    //AUTORIZATION, //this is not a real state, we enter the state machine only after that we have been autorizated.
    //    IDLE,           //this state is the first state after autorization, in this state the Device is waiting for command from the remote. can be reached also from DATA_EXPORT state.
    //    SHUT_DOWN,      //this state cause to shut down of the device. it can be reached only from IDLE state. TBD - think who will turn on the device in this case.
    //    POSITIONIG,     //this state is responsible on the positing of the patient eye. can reached from IDLE state to start measure before the measurement start and from MEASURING state during the measurement.
    //    MANUAL          //
    //    MEASURING,      //this state is responsible on the measuring of the patient, can be reached from POSITIONIG state and from himself (MEASURING state).
    //    DATA_EXPORT     //this state is responsible on the exporting of the measurment date, can be reach only after MEASURING state.

    public sealed class KeyStates
    {
        public static Command[] Idle = 
        {
            Command.SETTINGS,
            Command.ADD_NEW_PATIENT //patient
        };

        public static Command[] Positioning = 
        {
            Command.SWITCH_EYE,     //SEND WITH EyeSide
            Command.CHOOSE_PATIENT_AGE, //SEND WITH PatientMeaurementType
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
        public static Command[] Manual= 
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

    public enum commands
    {
        //idle commands
        SETTINGS,
        ADD_NEW_PATIENT,

        //positioning commands
        SWITCH_EYE,     //SEND WITH EyeSide
        CHOOSE_PATIENT_AGE, //SEND WITH PatientMeaurementType ADULT\CHILD
        GO_TO_IDLE,
        EXAM_START,
        CR_UP,
        CR_DOWN,
        MOVE_UP,
        MOVE_DOWN,
        MOVE_LEFT,
        MOVE_RIGHT,
        FOCUS_IN,
        FOCUS_OUT,
        FOCUS_HOME,
        SCREEN_CLICK,

        //manual command

        //measuring commands
        EXAM_AQUIRE,
        EXAM_SKIP,
        EXAM_ABORT,
        WF_GAIN_UP,
        WF_GAIN_DOWN,
        DIALOG_YES,
        DIALOG_NO,
        DIALOG_CANCEL,
        DIALOG_OK,

        //export commands
        EXPORD_TO_PDF,
        EXPORD_TO_XML
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
        TBDRA split ScreenClick & amountOfMM
    }

    //this class is the settings class, each time that Tessan want to change the settings they will send the whole structure.
    public class Settings
    {
        public string language;
    }

    //messages that send from VX650 to Tessan
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
