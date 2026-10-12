using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ValheimGatherer.Configuration;

namespace ValheimGatherer.Core
{
    public class ProximityScanner : MonoBehaviour
    {
        private readonly Collider[] _hitBuffer = new Collider[128];
        private readonly HashSet<int> _scannedObjectIds = new HashSet<int>();

        private void Start()
        {
            StartCoroutine(ScanRoutine());
        }

        private IEnumerator ScanRoutine()
        {
            while (true)
            {
                var config = GathererManager.Instance.Config;
                if (config == null || !config.Enabled.Value)
                {
                    yield return new WaitForSeconds(3.0f);
                    continue;
                }

                var mode = config.Mode.Value;
                if (mode != DiscoveryMode.InteractAndProximity && mode != DiscoveryMode.All)
                {
                    yield return new WaitForSeconds(3.0f);
                    continue;
                }

                if (Player.m_localPlayer == null || Player.m_localPlayer.IsDead() || Minimap.instance == null)
                {
                    yield return new WaitForSeconds(2.0f);
                    continue;
                }

                PerformScan(config.ProximityRadius.Value);

                float interval = Mathf.Max(1.0f, config.ProximityScanInterval.Value);
                yield return new WaitForSeconds(interval);
            }
        }

        private void PerformScan(float radius)
        {
            Vector3 center = Player.m_localPlayer.transform.position;
            int count = Physics.OverlapSphereNonAlloc(center, radius, _hitBuffer);

            _scannedObjectIds.Clear();

            for (int i = 0; i < count; i++)
            {
                var col = _hitBuffer[i];
                if (col == null) continue;

                var go = col.gameObject;
                int id = go.GetInstanceID();
                if (_scannedObjectIds.Contains(id))
                    continue;

                _scannedObjectIds.Add(id);
                GathererManager.Instance.TryProcessGameObject(go);
            }
        }
    }
}
