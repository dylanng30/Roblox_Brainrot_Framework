using System.Collections;
using UnityEngine;

namespace Dylanng
{
    public abstract class BaseLoadingOverlay : EntityBase
    {
        public abstract IEnumerator StartLoad();
        public abstract IEnumerator EndLoad();
    }
}