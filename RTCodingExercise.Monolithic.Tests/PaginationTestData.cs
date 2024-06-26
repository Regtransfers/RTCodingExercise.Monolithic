using System.Collections;
using System.Collections.Generic;

namespace RTCodingExercise.Monolithic.Tests;

public class PaginationTestData : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        for (var i = 1; i <= 5; i++)
        {
            yield return new object[] { i };
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}