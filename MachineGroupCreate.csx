//get the active machine group
var	activeGroup = Mastercam.Support.GroupManager.GetActiveMachineGroup();

if(activeGroup == null)
{
Mastercam.Support.GroupManager.NewGroup(Mastercam.Support.MachineDefManager.GetCurrentMachineType());
}