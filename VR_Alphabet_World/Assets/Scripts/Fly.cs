using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Fly : MonoBehaviour
{
    public static int contor = 0; // Indice pentru litera curentă
    public static int contor2 = 0; // Contor pentru numărul de litere zburate
    public static int level = 1; // Nivelul curent

    [SerializeField] private bool destroy;
    [SerializeField] private UnityEvent onTrigger;
    [SerializeField] private UnityEvent onExit;
    [SerializeField] private string tag;
    [SerializeField] private AudioSource flyAudio;
    [SerializeField] private NarratorSpeaking narrator;
    [SerializeField] private Transform letterCharactersForest; // Containerul pentru litere
    [SerializeField] private Collider[] colliders; // Collider-ele pentru activare

    private List<GameObject> letters = new List<GameObject>(); // Lista cu literele din container
    private Rigidbody rb; // Rigidbody-ul literei curente
    private GameObject currentLetterObject; // Litera curentă
    private Collider colliderLetter; // Collider-ul curent activ
    private char letter; // Litera curentă
    private int maxLettersPerLevel = 6; // Numărul maxim de litere pe nivel

    void Start()
    {
        // Configurare inițială
        if (letterCharactersForest == null)
        {
            Debug.LogError("Containerul letterCharactersForest nu este setat!");
            return;
        }

        foreach (Transform child in letterCharactersForest)
        {
            letters.Add(child.gameObject);
        }

        Debug.Log($"Am găsit {letters.Count} litere în letterCharactersForest.");

        if (colliders.Length == 0)
        {
            Debug.LogError("Nu există collider-e setate!");
            return;
        }

        InitializeCurrentLetter();
    }

    void OnTriggerEnter(Collider other)
    {
        if (!string.IsNullOrEmpty(tag) && !other.CompareTag(tag))
        {
            return;
        }

        onTrigger?.Invoke();

        if (rb != null)
        {
            flyAudio.Play();
            rb.isKinematic = false;
            rb.linearVelocity = new Vector3(0, 25f, 0); // Setăm viteza de zbor

            contor2++;
            contor++;

            // Dacă toate literele pentru nivelul curent sunt procesate
            if (contor2 ==6)
            {
                contor2 = 0; // Resetăm contorul pentru literele zburate
                level++; // Trecem la nivelul următor
                Debug.Log($"Nivelul curent: {level}");

                if (level > 3) // Exemplu: 2 niveluri maxime
                {
                    Debug.Log("Toate nivelurile au fost completate!");
                    return;
                }

                StartCoroutine(narrator.TeleportBack());
            }

            if (contor >= letters.Count)
            {
                Debug.LogWarning("Toate literele au fost procesate. Resetez contorul la 0.");
                contor = 0; // Resetăm contorul
            }

            InitializeCurrentLetter();
        }

        if (destroy)
        {
            Destroy(gameObject);
        }
    }

    private void InitializeCurrentLetter()
    {
        if (level == 1)
        {
            maxLettersPerLevel = 6; // Nivelul 1: literele A-F
        }
        else if (level == 2)
        {
            maxLettersPerLevel = 6; 
        }

        if (contor < letters.Count && letters[contor] != null)
        {
            currentLetterObject = letters[contor];
            currentLetterObject.SetActive(true);

            int colliderIndex = contor % colliders.Length;
            colliderLetter = colliders[colliderIndex];
            colliderLetter.enabled = true;

            rb = currentLetterObject.GetComponentInChildren<Rigidbody>();
        }

        for (int i = 0; i < letters.Count; i++)
        {
            if (letters[i] != null && letters[i] != currentLetterObject)
            {
                letters[i].SetActive(false);
            }
        }

        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i] != null && colliders[i] != colliderLetter)
            {
                colliders[i].enabled = false;
            }
        }
    }
}