# 🏧 Man in the ATM (Prototype)

**Man in the ATM** is a surreal first-person simulation/thriller where the player inhabits the claustrophobic interior of an automated teller machine. Subverting traditional simulation tropes, the player acts as the "human processor" behind every transaction, balancing accuracy and speed in a distopian setting.

---

## 📸 Screenshots & Visuals





| ATM Interior & Diegetic UI | Interaction & Feedback |
|:---:|:---:|
| ![Interior Placeholder](https://via.placeholder.com/400x225?text=ATM+Interior+View) | ![Feedback Placeholder](https://via.placeholder.com/400x225?text=Interaction+Outline+Effect) |
| ![Screenshot](Screenshots/screenshot1.png)| ![Screenshot](Screenshots/screenshot2.png)  ![Screenshot](Screenshots/screenshot3.png)|

---

## 🎮 Key Features

- **Surreal Interaction Model:** Experience a restricted, stationary POV that emphasizes claustrophobia and industrial isolation.
- **Diegetic UI System:** Crucial game data is integrated directly into the 3D environment (ATM screens), removing the need for a traditional HUD and deepening immersion.
- **Dynamic Transaction Logic:** A procedural system that generates transaction requests in multiples of 5, scaling in complexity.
- **Professional Feedback Loop:** Includes shader-based object outlining, dynamic crosshair color states, and physical visual cues for held currency.

---

## 🛠️ Technical Stack

- **Engine:** Unity 2022.3+ (Universal Render Pipeline - URP)
- **Language:** C#
- **Input System:** Unity New Input System (Action-based)
- **Key Concepts:** Raycasting, Modular C# Architecture, Diegetic UI Design, Shader-based Visual Feedback.

---

## 🚀 Implementation Details

### **Interaction System**
Using **Raycasting**, the system detects specific components like `ParaDegeri` (Money Value) and `OutputSlot`. The interaction is handled via **Input Action Assets**, making the game compatible with various input devices (Mouse, Gamepad, etc.).

### **Validation Logic**
```csharp
// Example of the transaction verification logic
if (currentHeldMoney == requestedAmount) {
    // Success: Generate new value, reset UI to white
} else {
    // Failure: Flash UI red, reset player inventory
}
