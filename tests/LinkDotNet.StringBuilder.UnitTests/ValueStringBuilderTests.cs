﻿using System;

namespace LinkDotNet.StringBuilder.UnitTests;

public class ValueStringBuilderTests
{
    [Fact]
    public void ShouldThrowIndexOutOfRangeWhenStringShorterThanIndex()
    {
        var stringBuilder = new ValueStringBuilder();
        stringBuilder.Append("Hello");

        try
        {
            _ = stringBuilder[50];
        }
        catch (IndexOutOfRangeException)
        {
            Assert.True(true);
            return;
        }

        Assert.False(true);
    }

    [Fact]
    public void ShouldTryToCopySpan()
    {
        var stringBuilder = new ValueStringBuilder();
        stringBuilder.Append("Hello");
        var mySpan = new Span<char>(new char[5], 0, 5);

        var result = stringBuilder.TryCopyTo(mySpan);

        result.Should().BeTrue();
        mySpan.ToString().Should().Be("Hello");
    }

    [Fact]
    public void ShouldReturnSpan()
    {
        var stringBuilder = new ValueStringBuilder();
        stringBuilder.Append("Hello");

        var output = stringBuilder.AsSpan().ToString();

        output.Should().Be("Hello");
    }

    [Fact]
    public void ShouldReturnLength()
    {
        var stringBuilder = new ValueStringBuilder();
        stringBuilder.Append("Hello");

        var length = stringBuilder.Length;

        length.Should().Be(5);
    }

    [Fact]
    public void ShouldClear()
    {
        var stringBuilder = new ValueStringBuilder();
        stringBuilder.Append("Hello");

        stringBuilder.Clear();

        stringBuilder.Length.Should().Be(0);
        stringBuilder.ToString().Should().Be(string.Empty);
    }

    [Fact]
    public void ShouldReturnEmptyStringWhenInitialized()
    {
        var stringBuilder = new ValueStringBuilder();

        stringBuilder.ToString().Should().Be(string.Empty);
    }

    [Fact]
    public void ShouldRemoveRange()
    {
        var stringBuilder = new ValueStringBuilder();
        stringBuilder.Append("Hello World");

        stringBuilder.Remove(0, 6);

        stringBuilder.Length.Should().Be(5);
        stringBuilder.ToString().Should().Be("World");
    }

    [Theory]
    [InlineData(-1, 2)]
    [InlineData(1, -2)]
    [InlineData(90, 1)]
    public void ShouldThrowExceptionWhenOutOfRangeIndex(int startIndex, int length)
    {
        var stringBuilder = new ValueStringBuilder();
        stringBuilder.Append("Hello");

        try
        {
            stringBuilder.Remove(startIndex, length);
        }
        catch (ArgumentOutOfRangeException)
        {
            Assert.True(true);
            return;
        }

        Assert.False(true);
    }

    [Fact]
    public void ShouldNotRemoveEntriesWhenLengthIsEqualToZero()
    {
        var stringBuilder = new ValueStringBuilder();
        stringBuilder.Append("Hello");

        stringBuilder.Remove(0, 0);

        stringBuilder.ToString().Should().Be("Hello");
    }

    [Fact]
    public unsafe void ShouldGetPinnableReference()
    {
        var stringBuilder = new ValueStringBuilder();
        stringBuilder.Append("Hey");

        fixed (char* c = stringBuilder)
        {
            c[0].Should().Be('H');
            c[1].Should().Be('e');
            c[2].Should().Be('y');
        }
    }
}