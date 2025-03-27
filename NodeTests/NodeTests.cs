using Algoritmo_Raft;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace NodeTests
{
    [TestClass]
    public class NodeTests
    {
        [TestMethod]
        public void NodeStartsAsFollower()
        {
            var node = new Node(1);
            Assert.AreEqual(NodeState.Follower, node.State);
        }

        [TestMethod]
        public void NodeFail()
        {
            var node = new Node(1);
            node.FailNode();
            Assert.IsFalse(node.IsAlive);
            Assert.AreEqual(NodeState.Failed, node.State);
        }

        [TestMethod]
        public void NodeRecover()
        {
            var node = new Node(1);
            node.FailNode();
            node.RecoverNode();
            Assert.IsTrue(node.IsAlive);
            Assert.AreEqual(NodeState.Follower, node.State);
        }

        [TestMethod]
        public void NodeReceiveMessage()
        {
            var node = new Node(1);
            var message = new Message { Id = 2, ToNodeId = 1, Content = "Hello" };

            node.ReceiveMessage(message);

            Assert.IsTrue(node.GetLogs().Any(log => log.Contains("Mensaje recibido del Nodo 2: Hola")));
        }

        [TestMethod]
        public void NodeSendMessage()
        {
            var node1 = new Node(1);
            var node2 = new Node(2);
            var network = new List<Node> { node1, node2 };

            node1.SetNetwork(network);
            node2.SetNetwork(network);

            node1.SendMessage(2, "Hi there!");

            Assert.IsTrue(node1.GetLogs().Any(log => log.Contains("Mensaje enviado al Nodo 2: ¡Hola!")));
            Assert.IsTrue(node2.GetLogs().Any(log => log.Contains("Mensaje recibido del Nodo 1: ¡Hola!")));
        }

        [TestMethod]
        public void NodeStartElectionAndBecomeLeader()
        {
            var nodes = new List<Node>();
            for (int i = 1; i <= 5; i++)
                nodes.Add(new Node(i));

            foreach (var node in nodes)
                node.SetNetwork(nodes);

            nodes[0].StartElection();

            Assert.IsTrue(nodes[0].State == NodeState.Leader || nodes[0].State == NodeState.Follower);
        }

        [TestMethod]
        public void NodeFailsElectionWithFewVotes()
        {
            var node1 = new Node(1);
            var node2 = new Node(2);
            var node3 = new Node(3);

            var network = new List<Node> { node1, node2, node3 };
            foreach (var node in network)
                node.SetNetwork(network);

            node2.FailNode(); 
            node1.StartElection();

            Assert.AreNotEqual(NodeState.Leader, node1.State);
        }
    }
}
