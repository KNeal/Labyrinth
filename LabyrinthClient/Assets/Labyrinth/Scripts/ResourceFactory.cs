
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Labyrinth
{
    public class ResourceFactory
    {
        const string PlayerPrefab = "Player";

        public static GameObject CreatePlayer(Transform transform)
        {
            return PhotonNetwork.Instantiate(PlayerPrefab, transform.position, transform.rotation, 0);

        }
    }
}
