using NUnit.Framework;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.TestTools;

[TestFixture]
public class PlayerMovementTests
{
    [UnityTest]
    public IEnumerator PlayerMovesRight()
    {
        GameObject playerObject = new GameObject();
        PlayerMovement playerMovement = playerObject.AddComponent<PlayerMovement>();

        float initialPositionX = playerObject.transform.position.x;

        // Simulate moving right
        playerMovement.Update();
        playerMovement.SetInput(1f);
        yield return null; // Wait for the next frame

        float newPositionX = playerObject.transform.position.x;

        Assert.Greater(newPositionX, initialPositionX);
    }

    [UnityTest]
    public IEnumerator PlayerJumps()
    {
        GameObject playerObject = new GameObject();
        PlayerMovement playerMovement = playerObject.AddComponent<PlayerMovement>();

        bool jumpAudioPlayed = false;

        // Replace the jump audio source with a mock
        playerMovement.SetJumpAudioSource(new MockJumpAudioSource(() => jumpAudioPlayed = true));

        // Simulate jumping
        playerMovement.Update();
        playerMovement.SetInput(0f, true);
        yield return null; // Wait for the next frame

        Assert.True(jumpAudioPlayed);
    }

    // Add more tests for other functionalities as needed
}

// Mock class for testing the jump audio source
public class MockJumpAudioSource : IJumpAudioSource
{
    private readonly Action onPlayCallback;

    public MockJumpAudioSource(Action onPlayCallback)
    {
        this.onPlayCallback = onPlayCallback;
    }

    public void Play()
    {
        onPlayCallback?.Invoke();
    }
}

// Interface for the jump audio source
public interface IJumpAudioSource
{
    void Play();
}
