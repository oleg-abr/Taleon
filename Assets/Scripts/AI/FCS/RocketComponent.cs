﻿using UnityEngine;

namespace Assets.Scripts.AI.FCS
{
	[RequireComponent(typeof(Rigidbody), typeof(Collider))]
	public class RocketComponent : MonoBehaviour
	{
		private Rigidbody _body;
		private float _lifeTime;

		/// <summary>
		///     The acceleration of this rocket in units per s².
		/// </summary>
		public float Acceleration;

		/// <summary>
		///     The amount of time it takes after launch until the rocket is activated.
		/// </summary>
		public float ActivationTime;

		/// <summary>
		///     The amount of time this rockets can burn before all fuel is gone.
		/// </summary>
		public float Burntime;

		/// <summary>
		///     The amount of velocity this rocket starts with.
		/// </summary>
		public float InitialVelocity;
		
		/// <summary>
		///     The amount of time this rocket lifes until its destroyed.
		/// </summary>
		/// <remarks>
		///     Should be greater or equal to <see cref="Burntime" />.
		/// </remarks>
		public float Lifetime;

		public GameObject target;
		private bool _isActivated;
		private Collider _collider;
		private GameObject _engine;

		// Use this for initialization
		private void Start()
		{
			_body = GetComponent<Rigidbody>();
			_collider = GetComponent<Collider>();
			_collider.enabled = false;
			_engine = transform.FindChild("Engine").gameObject;
			
			_body.velocity = transform.forward * InitialVelocity;
		}

		// Update is called once per frame
		private void Update()
		{
			_lifeTime += Time.deltaTime;

			if (_lifeTime >= ActivationTime && !_isActivated)
			{
				ActivateEngine();
			}

			var direction = CalculateDirectionToTarget();
			if (_isActivated && _lifeTime < Burntime && target != null)
			{
				BurnEngine(direction);
			}
			else
			{
				StopEngine();
			}

			transform.LookAt(transform.position + direction);

			if (_lifeTime >= Lifetime)
				Destroy(gameObject);
		}

		private void StopEngine()
		{
			_engine.SetActive(false);
		}

		private void ActivateEngine()
		{
			_isActivated = true;
			_engine.SetActive(true);
			_collider.enabled = true;
		}

		private void BurnEngine(Vector3 direction)
		{
			var force = _body.mass * Acceleration;
			_body.AddForce(direction * force);
		}

		private Vector3 CalculateDirectionToTarget()
		{
			if (target == null)
				return Vector3.zero;

			// TODO: Find projected position of target based on its current acceleration
			var delta = target.transform.position - transform.position;
			var direction = delta.normalized;
			return direction;
		}

		private void OnTriggerEnter(Collider other)
		{
			Explode();
		}

		private void Explode()
		{
			Destroy(gameObject);
		}
	}
}