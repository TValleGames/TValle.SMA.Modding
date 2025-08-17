using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.TValle.Tools.Runtime.UI
{
    [Model]
    public class JobWithClientDefaultMenuModel 
    {
        public event Action<JobWithClientDefaultMenuModel> onShowModelInfo;
        public event Action<JobWithClientDefaultMenuModel> onShowClientInfo;
        public event Action<JobWithClientDefaultMenuModel> onLeave;

       
        [Label("Model Curriculum", Language.en)]
        [Description("-Click here to see the model's measurements, interests, and current job skill.", Language.en)]       
        public void ShowModelInfo()
        {
            onShowModelInfo?.Invoke(this);
        }

        [Label("Client Info", Language.en)]
        [Description("-Click here to see some information about the current client.", Language.en)]
        public void ShowClientInfo()
        {
            onShowClientInfo?.Invoke(this);
        }

        [Label("Leave", Language.en)]
        [Description("-Exit the room and continue on with the rest of the day.", Language.en)]
        public void Leave()
        {
            onLeave?.Invoke(this);
        }
    }
    //TODO: Coworker 
    [Model]
    public class JobWithEmployerDefaultMenuModel
    {
        public event Action onShowModelInfo;
        public event Action onShowEmployerInfo;
        public event Action onModelDismissed;

     
        [Label("Model Curriculum", Language.en)]
        [Description("-Click here to see the model's measurements, interests, and current job skill.", Language.en)]
        public void ShowModelInfo()
        {
            onShowModelInfo?.Invoke();
        }

        [Label("Employer Info", Language.en)]
        [Description("-Click here to see some information about the current employer.", Language.en)]
        public void ShowClientInfo()
        {
            onShowEmployerInfo?.Invoke();
        }

        [Label("Conclude Assignment", Language.en)]
        [Description("-Concludes the assignment.", Language.en)]
        public void DismissClient()
        {
            onModelDismissed?.Invoke();
        }
    }
    [Model]
    public class JobWithEmployerModelGoneDefaultMenuModel
    {
        public event Action onShowEmployerInfo;
        public event Action onEndSession;
       


        [Label("Employer Info", Language.en)]
        [Description("-Click here to see some information about the current employer.", Language.en)]
        public void ShowClientInfo()
        {
            onShowEmployerInfo?.Invoke();

        }
        [Label("Conclude Assignment", Language.en)]
        [Description("-Concludes the assignment.", Language.en)]
        public void EndSession()
        {
            onEndSession?.Invoke();

        }
    }
}
