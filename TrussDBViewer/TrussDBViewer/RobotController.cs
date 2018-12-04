using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ABB.Robotics.Controllers;
using ABB.Robotics.Controllers.Discovery;
using ABB.Robotics.Controllers.RapidDomain;

namespace TrussDBViewer
{
    public class RobotController
    {
        public List<ControllerInfo> ControllerScanner()
        {
            //Finds controllers on the system
            
            NetworkScanner scanner = new NetworkScanner();
            scanner.Scan();
            ControllerInfoCollection controllers = scanner.Controllers;
            
            List<ControllerInfo> foundControllers = new List<ControllerInfo>();

            foreach(ControllerInfo controller in controllers)
            {
                foundControllers.Add(controller);
            }
            return foundControllers;
        }

        public void SetRobotTarget(Controller controller)
        {
       
            RapidData rdSetRobTarget = controller.Rapid.GetRapidData("T_ROB1", "RobotTargets");
            //ABB.Robotics.Controllers.RapidDomain.RobTarget rtSetTarget;
            //This is the definition for rtStart in RobotStudio
            //CONST robtarget rtStart:=[[-685.486,-13.459,1280.47],[0,1,0,0],[-1,-4,4,1],[0,9E+09,9E+09,9E+09,9E+09,9E+09]];

        
            controller.Logon(UserInfo.DefaultUser);
            //rdSetRobTarget.Value = [-685.486, -13.459, 1280.47];

            RapidData rdSetAnotherRobTarget = controller.Rapid.GetRapidData("T_ROB1", "RobotTargets", "rtHome");
            //Assign new value to .Net variable
            //rapidBool.Value = false;
            //Request mastership of Rapid before writing to the controller
            //this.master = Mastership.Request(this.aController.Rapid);
            //Change: controller is repaced by aController
            //rd.Value = rapidBool;
            //Release mastership as soon as possible
            //this.master.Dispose();
        }

        public List<ABB.Robotics.Controllers.RapidDomain.RobTarget> GetRobotTargets(Controller controller)
        {
            RapidData rdRobotTargets = controller.Rapid.GetRapidData("T_ROB1", "RobotTargets");
            
            ABB.Robotics.Controllers.RapidDomain.RobTarget rtTargets;
            List<ABB.Robotics.Controllers.RapidDomain.RobTarget> lstRobTargets = new List<RobTarget>();
            if (rdRobotTargets.Value is ABB.Robotics.Controllers.RapidDomain.RobTarget)
            {
                rtTargets = (ABB.Robotics.Controllers.RapidDomain.RobTarget)rdRobotTargets.Value;
                lstRobTargets.Add(rtTargets);
                return lstRobTargets;
            }
            else
            {
                return lstRobTargets;
            }
        }

        //public ABB.Robotics.Controllers.RapidDomain.RobTarget GetRobotTarget(Controller controller)
        //{

            //RapidData rdRobTarget = controller.Rapid.GetRapidData("T_ROB1", "RobotTargets", "rtStart");
            //ABB.Robotics.Controllers.RapidDomain.RobTarget rtStart;
            
            //if(rdRobTarget.Value is ABB.Robotics.Controllers.RapidDomain.RobTarget)
            //{
            //    rtStart = (ABB.Robotics.Controllers.RapidDomain.RobTarget)rdRobTarget.Value;
            //    return rtStart;
            //}
            //else
            //{
            //    return rtStart = RobTarget.Empty;
            //}
        //}
    }
}
