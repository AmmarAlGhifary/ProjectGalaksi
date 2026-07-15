# Project Galaksi AR

Aplikasi edukasi Augmented Reality Tata Surya berbasis Unity dan Vuforia Engine.

## Kebutuhan Sistem

- Unity Editor versi 6000.5.2f1 (atau yang lebih baru)
- Vuforia Engine versi 11.4.4
- File com.ptc.vuforia.engine-11.4.4.tgz (didapat secara terpisah)

## Cara Melakukan Clone dan Menjalankan Proyek

Dikarenakan batas ukuran file maksimum dari GitHub, file inti Vuforia Engine (yang berukuran lebih dari 100 MB) tidak ikut disertakan di dalam repositori ini. Jika Anda langsung membuka proyek ini setelah di-clone, Unity akan menampilkan pesan error kehilangan dependensi Vuforia.

Untuk menjalankan proyek ini dengan benar, ikuti urutan langkah berikut:

1. Buka terminal atau command prompt, lalu clone repositori ini ke komputer Anda:
   ```bash
   git clone https://github.com/AmmarAlGhifary/ProjectGalaksi.git
   ```

2. Siapkan file package Vuforia. Anda memerlukan file `com.ptc.vuforia.engine-11.4.4.tgz`. File ini dapat diunduh melalui Vuforia Developer Portal atau diminta dari pemilik proyek.

3. Salin file `com.ptc.vuforia.engine-11.4.4.tgz` tersebut ke dalam folder `Packages/` yang berada di dalam direktori `ProjectGalaksi` yang baru saja Anda clone. 

4. Pastikan lokasi akhirnya menjadi seperti ini: `ProjectGalaksi/Packages/com.ptc.vuforia.engine-11.4.4.tgz`.

5. Buka Unity Hub. Klik tombol **Add** (atau "Add project from disk"), kemudian pilih folder `ProjectGalaksi`.

6. Buka proyek tersebut melalui Unity Hub. Unity akan mendeteksi file .tgz yang baru saja dimasukkan, melakukan kompilasi ulang, dan memuat semua pengaturan tanpa error.

7. Buka scene utama yang berada di folder `Assets/Scenes/SampleScene.unity`.

## Catatan

File `.tgz` sudah dikecualikan dari version control melalui `.gitignore`. Anda tidak perlu khawatir file tersebut akan ikut terunggah jika Anda melakukan commit dan push ke repositori ini di masa mendatang.
