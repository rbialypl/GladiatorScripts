//Each weapon must have block and attack methods
//They must also have damage values and must wait a certain time before they can attack again
using UnityEngine;
public abstract class Weapon : MonoBehaviour{

	Transform owner;
	float damage;
	float timeBetweenAttacks;
	bool attacking;
	bool blocking;
	
	public abstract void Attack(int type);
	public abstract void Block();
	public abstract void dealDamage(GameObject target);
}
