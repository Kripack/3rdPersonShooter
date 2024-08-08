using System.Collections.Generic;
using UnityEngine;

public class AmmoManager
{
    private Dictionary<AmmoType, int> _currentAmmo;
    private Dictionary<AmmoType, int> _totalAmmo;
    private Dictionary<AmmoType, int> _magazineSizes;

    public void Initialize()
    {
        _currentAmmo = new Dictionary<AmmoType, int>
        {
            { AmmoType.Pistol9Mm, 9 },
            { AmmoType.Rifle762X39Mm, 30 },
        };

        _totalAmmo = new Dictionary<AmmoType, int>
        {
            { AmmoType.Pistol9Mm, 90 },
            { AmmoType.Rifle762X39Mm, 180 },
        };

        _magazineSizes = new Dictionary<AmmoType, int>
        {
            { AmmoType.Pistol9Mm, 9 },
            { AmmoType.Rifle762X39Mm, 30 },
        };
    }

    public bool UseAmmo(AmmoType ammoType)
    {
        if (_currentAmmo[ammoType] > 0)
        {
            _currentAmmo[ammoType]--;
            return true;
        }
        else return false;
    }

    public void Reload(AmmoType ammoType)
    {
        int ammoNeeded = _magazineSizes[ammoType] - _currentAmmo[ammoType];
        if (_totalAmmo[ammoType] >= ammoNeeded)
        {
            _currentAmmo[ammoType] += ammoNeeded;
            _totalAmmo[ammoType] -= ammoNeeded;
        }
        else
        {
            _currentAmmo[ammoType] += _totalAmmo[ammoType];
            _totalAmmo[ammoType] = 0;
        }
    }

    public int GetCurrentAmmo(AmmoType ammoType)
    {
        return _currentAmmo[ammoType];
    }

    public int GetTotalAmmo(AmmoType ammoType)
    {
        return _totalAmmo[ammoType];
    }

    public void AddTotalAmmo(AmmoType ammoType, int count)
    {
        _totalAmmo[ammoType] += count;
    }
}
