﻿using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ProjectileComponent : MonoBehaviour
{
	private float _currentLifetime;
	private float _lifetime;

	public float range;
	public float velocity;

	private void Start()
	{
		_lifetime = range / velocity;
	}

	public void Shoot(Vector3 position, Vector3 direction)
	{
		var body = GetComponent<Rigidbody>();
		transform.position = position;
		body.MovePosition(position);
		body.velocity = velocity * direction;
	}

	// Update is called once per frame
	private void Update()
	{
		_currentLifetime += Time.deltaTime;
		if (_currentLifetime >= _lifetime)
			Destroy(gameObject);
	}
}