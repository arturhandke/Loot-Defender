using System.Collections;
using System.Collections.Generic;

public struct Position {
	public int x, y;
	public Position(int x, int y)
	{
		this.x = x;
		this.y = y;
	}

	public override bool Equals(object obj)
	{
		if (GetHashCode() == obj.GetHashCode())
			return true;
		return false;
	}

	public override int GetHashCode()
	{
		unchecked
		{
			int hash = 47;
			hash = hash * 227 + x.GetHashCode();
			hash = hash * 227 + y.GetHashCode();
			return hash;
		}
	}
}
