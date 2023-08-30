using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBox : HittableFromBelow
{
    [SerializeField] GameObject _itemPrefab;
    [SerializeField] Vector2 _itemLaunchVelocity;
    GameObject _item;
    bool _used;

    void Start()
    {
        _item?.SetActive(false);
    }

    protected override bool CanUse => _used == false;

    protected override void Use()
    {
        _item = Instantiate(_itemPrefab,
            transform.position + new Vector3(0, 0.7f, 0),
            Quaternion.identity,
            transform);
        if (_item == null)
            return;

        _used = true;
        _item.SetActive(true);
        var itemRigidbody = _item.GetComponent<Rigidbody2D>();
        if (itemRigidbody != null)
        {
            itemRigidbody.velocity = _itemLaunchVelocity;
        }
    }

}
