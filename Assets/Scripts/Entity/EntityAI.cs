using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityAI : MonoBehaviour
{
    public BoatEntity entity;


    public class CommandType
    {
        public CommandType(Command c, GameObject e, Vector3 p)
        {
            command = c;
            pointToFollow = p;
        }
        public Command command;
        public Vector3 pointToFollow;
    }

    public enum Command { GoToPoint }

    public List<CommandType> commands = new List<CommandType>();
    public Vector3 pointToFollow;


    public virtual void OnStart()
    {

    }


    public void OnUpdate(float dt)
    {
        ExecuteCommand(dt); // Runs commands
    }

    public virtual void GoToPoint(float dt)
    {
        if (Vector3.SqrMagnitude(entity.parent.transform.position - commands[0].pointToFollow) < 25)
        {
            commands.RemoveAt(0);
        }
        else
        {
            entity.physics.desiredHeading = Mathf.Atan2(commands[0].pointToFollow.x - entity.physics.position.x, commands[0].pointToFollow.z - entity.physics.position.z);
            entity.physics.desiredSpeed = entity.physics.maxSpeed;
        }
    }

    public void ExecuteCommand(float dt)
    {
        if (commands.Count == 0)
        {
            entity.physics.desiredHeading = entity.physics.heading;
            entity.physics.desiredSpeed = 0;
        }
        else
        {
            switch (commands[0].command)
            {
                case Command.GoToPoint:
                    GoToPoint(dt);
                    break;
            }
        }
    }
}
