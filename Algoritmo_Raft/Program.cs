using System;
using System.Collections.Generic;
using System.Linq;

namespace Algoritmo_Raft
{
    public enum NodeState { Follower, Candidate, Leader, Failed }

    public class Message
    {
        public int Id { get; set; }
        public int ToNodeId { get; set; }
        public string Content { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }

    public class Node
    {
        public int Id { get; }
        public NodeState State { get; private set; } = NodeState.Follower;
        public List<string> Log { get; } = new List<string>();
        public bool IsAlive { get; private set; } = true;
        private List<Node> _network;
        private static readonly Random _rand = new Random();
        public IEnumerable<string> GetLogs() => Log;

        public Node(int id)
        {
            Id = id;
        }

        public void SetNetwork(List<Node> network)
        {
            _network = network;
        }

        public void FailNode()
        {
            IsAlive = false;
            State = NodeState.Failed;
            LogMessage("Nodo fallido.");
        }

        public void RecoverNode()
        {
            IsAlive = true;
            State = NodeState.Follower;
            LogMessage("Nodo recuperado.");
        }

        public void ReceiveMessage(Message msg)
        {
            if (!IsAlive) return;
            LogMessage($"Mensaje recibido de Node {msg.Id}: {msg.Content}");
        }

        public void SendMessage(int toNodeId, string content)
        {
            if (!IsAlive) return;
            var recipient = _network.FirstOrDefault(n => n.Id == toNodeId);
            if (recipient?.IsAlive == true)
            {
                var message = new Message { Id = Id, ToNodeId = toNodeId, Content = content };
                recipient.ReceiveMessage(message);
                LogMessage($"Mensaje enviado al nodo {toNodeId}: {content}");
            }
        }

        public void StartElection()
        {
            if (!IsAlive) return;
            State = NodeState.Candidate;
            LogMessage("Comenzaron las elecciones.");

            int votes = 1;
            foreach (var node in _network)
            {
                if (node.Id != Id && node.IsAlive && _rand.Next(0, 2) == 1)
                {
                    votes++;
                    node.LogMessage($"Votado por Node {Id}.");
                }
            }

            if (votes > _network.Count / 2)
            {
                State = NodeState.Leader;
                LogMessage("Elected as Leader.");
                foreach (var node in _network)
                    SendMessage(node.Id, "Nuevo líder elegido");
            }
            else
            {
                State = NodeState.Follower;
                LogMessage("Elecciones fallidas. Vuelve a ser seguidor.");
            }
        }

        public void LogMessage(string message)
        {
            var entry = $"[{DateTime.UtcNow}] Node {Id} ({State}): {message}";
            Log.Add(entry);
            Console.WriteLine(entry);
         
        }

    }

    class Program
    {
        static void Main()
        {
            var nodes = new List<Node>();
            for (int i = 1; i <= 5; i++)
                nodes.Add(new Node(i));

            foreach (var node in nodes)
                node.SetNetwork(nodes);

            Console.WriteLine("\n-- Simulación: Consenso exitoso --");
            nodes[0].StartElection();

            Console.WriteLine("\n-- Simulación: Fallo de un nodo --");
            nodes[1].FailNode();
            nodes[2].StartElection();

            Console.WriteLine("\n-- Simulación: Nodo recuperado --");
            nodes[1].RecoverNode();

            Console.WriteLine("\n-- Simulación: Partición de red (simulada por FailNode) --");
            nodes[3].FailNode();
            nodes[4].StartElection();
        }
    }
}
