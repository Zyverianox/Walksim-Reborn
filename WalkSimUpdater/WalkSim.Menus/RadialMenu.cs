using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using WalkSim.Animators;
using WalkSim.Plugin;
using WalkSim.Rigging;

namespace WalkSim.Menus;

public class RadialMenu : MonoBehaviour
{
    public struct Icon
    {
        public Image image;
        public Vector2 direction;
        public AnimatorBase animator;
    }

    public List<Icon> icons;
    private AnimatorBase selectedAnimator;
    private bool cursorWasLocked;
    private bool wasTurning;

    private void Awake()
    {
        Image component = transform.Find("Icons/Walk").GetComponent<Image>();
        Image component2 = transform.Find("Icons/Pose").GetComponent<Image>();
        Image component3 = transform.Find("Icons/Interact").GetComponent<Image>();
        Image component4 = transform.Find("Icons/Fly").GetComponent<Image>();
        icons = new List<Icon>
        {
            new Icon
            {
                image = component,
                direction = Vector2.up,
                animator = WalkSim.Plugin.Plugin.Instance.walkAnimator
            },
            new Icon
            {
                image = component3,
                direction = Vector2.left,
                animator = WalkSim.Plugin.Plugin.Instance.grabAnimator
            },
            new Icon
            {
                image = component2,
                direction = Vector2.down,
                animator = WalkSim.Plugin.Plugin.Instance.handAnimator
            },
            new Icon
            {
                image = component4,
                direction = Vector2.right,
                animator = WalkSim.Plugin.Plugin.Instance.flyAnimator
            }
        };
    }

    private void Update()
    {
        Vector2 mousePosition = Mouse.current.position.ReadValue();
        Vector2 screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);
        Vector2 direction = mousePosition - screenCenter;

        if (direction.magnitude < Screen.width / 20f)
        {
            return;
        }

        Icon closestIcon = default;
        float closestDistance = float.PositiveInfinity;
        foreach (Icon icon in icons)
        {
            float distance = Vector2.Distance(direction, icon.direction);
            if (distance < closestDistance)
            {
                closestIcon = icon;
                closestDistance = distance;
            }
        }

        selectedAnimator = closestIcon.animator;
        foreach (Icon icon in icons)
        {
            icon.image.color = icon.Equals(closestIcon) ? Color.white : Color.gray;
            icon.image.transform.localScale = Vector3.one * (icon.Equals(closestIcon) ? 1.5f : 1f);
        }
    }

    private void OnEnable()
    {
        cursorWasLocked = HeadDriver.Instance.LockCursor;
        wasTurning = HeadDriver.Instance.turn;
        HeadDriver.Instance.LockCursor = false;
        HeadDriver.Instance.turn = false;
    }

    private void OnDisable()
    {
        Logging.Debug("RadialMenu disabled");
        HeadDriver.Instance.LockCursor = cursorWasLocked;
        HeadDriver.Instance.turn = wasTurning;
        Rig.Instance.Animator = selectedAnimator;
        Logging.Debug("--Finished");
    }
}
