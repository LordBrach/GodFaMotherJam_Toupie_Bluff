using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

    /// <summary>
    /// Une étape de la SÉQUENCE de menu (pas une state machine : pas de transitions
    /// arbitraires, MenuFlow avance juste dans une liste ordonnée).
    /// Une étape = un panel + un fondu d'entrée/sortie + une condition de fin.
    /// Les enfants n'implémentent que OnEnter / OnTick et appellent CompleteStep().
    /// </summary>
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class MenuStep : MonoBehaviour
    {
        [Header("Transition")]
        [SerializeField, Min(0f)] protected float fadeDuration = 0.25f;

        [Header("Feedbacks, anims, sons)")]
        [SerializeField] private UnityEvent onEnter;
        [SerializeField] private UnityEvent onComplete;

        //Levé quand l'étape est terminée. Écouté par le MenuFlow
        public event Action<MenuStep> Completed;

        private CanvasGroup group;
        private bool active;

        //True uniquement quand l'étape est visible et joue : les inputs ne sont lus qu'ici
        protected bool IsActive => active;

        private void Update() { if (active) OnTick(); }

        private CanvasGroup Group
        {
            get
            {
                if (group == null) group = GetComponent<CanvasGroup>();
                return group;
            }
        }

        public IEnumerator Enter()
        {
            gameObject.SetActive(true);
            Group.alpha = 0f;
            Group.blocksRaycasts = false;

            OnEnter();
            onEnter?.Invoke();

            yield return UIFade.FadeTo(Group, 1f, fadeDuration);

            Group.blocksRaycasts = true;
            active = true;
        }

        public IEnumerator Exit()
        {
            active = false;
            Group.blocksRaycasts = false;
            OnExit();

            yield return UIFade.FadeTo(Group, 0f, fadeDuration);
            gameObject.SetActive(false);
        }

        /// <summary>À appeler par l'enfant quand la condition de passage est remplie.</summary>
        protected void CompleteStep()
        {
            if (!active) return;
            active = false;
            onComplete?.Invoke();
            Completed?.Invoke(this);
        }

        protected virtual void OnEnter() { }
        protected virtual void OnExit() { }
        protected virtual void OnTick() { }
    }