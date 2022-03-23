using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Infantry : BaseUnit
{
    [SerializeField] private float unitSpeed;
    private Vector3 target;

    private Coroutine activeCoroutine;

    public override void OrderManage()
    {
        if (isSelected)
        {
            if (activeCoroutine == null)
            {
                target = gameManager.Point;
                Vector3 direction = target - transform.position;
                activeCoroutine = StartCoroutine(MoveToTile(direction));
            }
        }
    }

    IEnumerator MoveToTile(Vector3 direction)
    {
        Vector3 start = transform.position;
        Vector3 target = map.GetNextTileInDirection(transform.position, direction).position;

        //update tabel with positions
        map.UpdateUnitPosition(transform, target, start);
        target.y += 0.5f;

        float percent = 0;
        while (percent < 1)
        {
            percent += unitSpeed * Time.deltaTime;
            transform.position = Vector3.Lerp(start, target, percent);

            yield return new WaitForSeconds(unitSpeed * Time.deltaTime);
        }
        activeCoroutine = null;
    }
}
