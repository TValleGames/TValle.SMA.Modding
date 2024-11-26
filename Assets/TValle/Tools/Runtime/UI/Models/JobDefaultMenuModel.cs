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
        public event Action<JobWithClientDefaultMenuModel> onShowObjectives;
        public event Action<JobWithClientDefaultMenuModel> onShowModelInfo;
        public event Action<JobWithClientDefaultMenuModel> onShowClientInfo;
        public event Action<JobWithClientDefaultMenuModel> onClientDismissed;

        [Label("Objectives", Language.en)]
        [Description("-A list of actions required to complete the current job session successfully.", Language.en)]
        public void ShowObjectives()
        {
            onShowObjectives?.Invoke(this);
        }
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

        [Label("Send the client off", Language.en)]
        [Description("-Start a conversation with the client to guide them to leave.", Language.en)]
        public void DismissClient()
        {
            onClientDismissed?.Invoke(this);
        }
    }
    //TODO: Coworker 
    [Model]
    public class JobWithEmployerDefaultMenuModel
    {
        public event Action<JobWithEmployerDefaultMenuModel> onShowObjectives;
        public event Action<JobWithEmployerDefaultMenuModel> onShowModelInfo;
        public event Action<JobWithEmployerDefaultMenuModel> onShowEmployerInfo;
        public event Action<JobWithEmployerDefaultMenuModel> onModelDismissed;

        [Label("Objectives", Language.en)]
        [Description("-A list of actions required to complete the current job session successfully.", Language.en)]
        public void ShowObjectives()
        {
            onShowObjectives?.Invoke(this);
        }
        [Label("Model Curriculum", Language.en)]
        [Description("-Click here to see the model's measurements, interests, and current job skill.", Language.en)]
        public void ShowModelInfo()
        {
            onShowModelInfo?.Invoke(this);
        }

        [Label("Employer Info", Language.en)]
        [Description("-Click here to see some information about the current employer.", Language.en)]
        public void ShowClientInfo()
        {
            onShowEmployerInfo?.Invoke(this);
        }

        [Label("Send the model off", Language.en)]
        [Description("-Start a conversation with the model to guide her to leave.", Language.en)]
        public void DismissClient()
        {
            onModelDismissed?.Invoke(this);
        }
    }
}
