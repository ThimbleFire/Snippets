// When recieving packets from the server they are passed through this buffer. This is neccesary as the network client socket runs on a separate thread and cannot interfere with Unity's update thread.
// You can send a packet through this event processor by calling EventProcess.AddInstructionParams( /*indexOfPacketNameRepresentedAsEnum*/ Inbound.ConnectionConfirmed, /*Method you want to fire*/ OnConfirmed );

using System;
using System.Collections.Generic;
using UnityEngine;

    public class EventProcessor : MonoBehaviour
    {
        // When the client needs to interact with the main thread, if a specific client packet is recieved, a delegate is fired
        public Dictionary<Inbound, Action<object[]>> instructionParams = new Dictionary<Inbound, Action<object[]>>();

        private List<Inbound> m_executingEvents = new List<Inbound>();
        private List<object[]> m_executingParams = new List<object[]>();
        private List<Inbound> m_queuedEvents = new List<Inbound>();
        private List<object[]> m_queuedParams = new List<object[]>();
        private System.Object m_queueLock = new object();

        public void AddInstructionParams( Inbound packet, Action<object[]> d )
        {
            instructionParams.Add( packet, d );
        }

        public void ClearInstructions()
        {
            instructionParams.Clear();
        }

        public void QueueEvent( Inbound action, params object[] m )
        {
            lock ( m_queueLock )
            {
                m_queuedEvents.Add( action );
                m_queuedParams.Add( m );
            }
        }

        public void RemoveInstructionParams( Inbound packet )
        {
            instructionParams.Remove( packet );
        }

        private void Awake()
        {
            DontDestroyOnLoad( this );
        }

        private void MoveQueuedEventsToExecuting()
        {
            lock ( m_queueLock )
            {
                while ( m_queuedEvents.Count > 0 )
                {
                    Inbound e = m_queuedEvents[0];
                    object[] p = m_queuedParams[0];

                    m_executingEvents.Add( e );
                    m_executingParams.Add( p );

                    m_queuedEvents.RemoveAt( 0 );
                    m_queuedParams.RemoveAt( 0 );
                }
            }
        }

        private void Update()
        {
            MoveQueuedEventsToExecuting();

            while ( m_executingEvents.Count > 0 )
            {
                Inbound action = m_executingEvents[0];
                object[] p = m_executingParams[0];

                m_executingEvents.RemoveAt( 0 );
                m_executingParams.RemoveAt( 0 );

                if ( instructionParams.ContainsKey( action ) )
                    instructionParams[action].Invoke( p );
                else
                {
                    Debug.LogError( string.Format( "No instruction for {0}.", action ) );
                }
            }
        }
    }
