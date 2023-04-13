using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Fusion.Sockets;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;
using System;
using System.Linq;

namespace Scripts.Network
{
    public class NetworkRunnerHandler : MonoBehaviour
    {
        [SerializeField] 
        private NetworkRunner _networkRunnerPrefab;

        private NetworkRunner _networkRunner;
        
        private void Start()
        {
            _networkRunner = Instantiate(_networkRunnerPrefab);
            _networkRunner.name = "Network_Runner";

            var clientTask = Initialize(_networkRunner, GameMode.AutoHostOrClient, NetAddress.Any(), SceneManager.GetActiveScene().buildIndex, null);
        }

        protected virtual Task Initialize(NetworkRunner runner, GameMode gameMode, NetAddress address, SceneRef scene, Action<NetworkRunner> initialized)
        {
            var sceneManager = runner.GetComponents(typeof(MonoBehaviour)).OfType<INetworkSceneManager>().FirstOrDefault();

            if (sceneManager == null)
            {
                sceneManager = runner.gameObject.AddComponent<NetworkSceneManagerDefault>();
            }

            runner.ProvideInput = true;

            return runner.StartGame(new StartGameArgs
            {
                GameMode = gameMode,
                Address = address,
                Scene = scene,
                SessionName = "TestRoom",
                Initialized = initialized,
                SceneManager = sceneManager
            });
        }
    }
}
