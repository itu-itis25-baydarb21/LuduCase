# CASE_Unity_Interaction_System

Unity ile geliştirilmiş, modüler, ölçeklenebilir ve görsel geri bildirimi yüksek bir **FPS Etkileşim Sistemi**. Bu proje, temiz kod prensipleri (SOLID), Interface tabanlı mimari ve Event-Driven (Olay Güdümlü) UI yönetimi kullanılarak hazırlanmıştır.

## Özellikler

* **Raycast Tabanlı Etkileşim:** Ekranın merkezinden gönderilen ışınlar ile nesnelerin hassas bir şekilde algılanması.
* **Polimorfik Etkileşim Mantığı:** `IInteractable` arayüzü sayesinde Kapılar, Sandıklar, Anahtarlar ve Düğmelerin (Switch) tek bir yapı üzerinden yönetilmesi.
* **Envanter Sistemi:** **ScriptableObjects** kullanılarak hazırlanan veri odaklı (Data-Driven) envanter yapısı.
* **Görsel Geri Bildirim (Visual Feedback):**
    * **Dinamik UI:** Nesnenin durumuna göre değişen metinler (Örn: "Press E to Open", "Locked", "Requires Key").
    * **Outline (Dış Hat) Sistemi:** Nesneye odaklanıldığında devreye giren ve nesnenin kendi rengini (Kırmızı/Yeşil) koruyan akıllı highlight sistemi.
    * **UI İkonları:** Toplanan eşyaların ekranda görsel olarak listelenmesi.
* **Gelişmiş Etkileşimli Nesneler:**
    * **Kapılar:** Tween benzeri kod ile animasyon, kilit mekanizması ve uzaktan kontrol (Remote Control) desteği.
    * **Sandıklar:** Kapak animasyonları ve durum yönetimi.
    * **Switch (Şalter):** **UnityEvents** kullanılarak, kod bağımlılığı olmadan (Decoupled) kapıları veya ışıkları tetikleyebilen yapı.

## Mimari ve Teknolojiler

* **Motor:** Unity 2022.3+
* **Dil:** C#
* **Tasarım Desenleri:** Observer Pattern (UI için), Component-Based Architecture.

### Temel Bileşenler

#### 1. Çekirdek (`InteractionSystem.Runtime.Core`)
* **`IInteractable`**: Tüm etkileşimli nesnelerin uyması gereken sözleşme.
* **`InteractableBase`**: Nesnelerin ortak özelliklerini (Odaklanma, Outline açma/kapama, ID yönetimi) barındıran soyut (abstract) sınıf.
* **`InteractionDetector`**: Oyuncunun baktığı nesneyi algılayan ve inputları işleyen mekanizma.

#### 2. Nesneler (`InteractionSystem.Runtime.Interactables`)
* **`ToggleInteractable`**: Aç/Kapa durumu olan nesneler (Kapı, Sandık) için temel mantık.
* **`Door`**: Kilit mantığına sahip kapı sınıfı. Belirli bir `ItemData` (Anahtar) gerektirebilir. Ayrıca `SetRemoteState` fonksiyonu ile Switch'ler tarafından uzaktan kontrol edilebilir.
* **`Switch`**: `UnityEvent` kullanarak Inspector üzerinden herhangi bir nesneyi tetikleyebilir. Bu sayede Switch kodu, Kapı kodunu bilmek zorunda kalmaz (Loose Coupling).

#### 3. Veri ve Oyuncu (`InteractionSystem.Runtime.Player`)
* **`ItemData` (ScriptableObject)**: Eşya verilerini (ID, İsim, İkon, Renk) tutar. Yeni eşya eklemek için kod yazmaya gerek yoktur.
* **`Inventory`**: Toplanan eşyaları listeler ve kapı kilitlerini kontrol eder.

#### 4. Arayüz (`InteractionSystem.Runtime.UI`)
* **`InteractionUI`**: Event'leri dinleyerek Crosshair, etkileşim yazıları ve ikonları anlık günceller. `Update()` döngüsünde gereksiz kontrol yapmaz.

## Proje Yapısı

```text
Assets/
├── Scripts/
│   └── Runtime/
│       ├── Core/           # Arayüzler, Base Class'lar, Detektörler
│       ├── Interactables/  # Somut sınıflar (Door, Switch, Chest)
│       ├── Player/         # Envanter, Kamera Kontrol
│       └── UI/             # HUD, İkonlar, Bildirimler
├── ScriptableObjects/
│   └── Items/              # Key_Red, Key_Blue verileri
└── Prefabs/                # Hazır etkileşimli objeler