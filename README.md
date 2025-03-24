# Robocode Cisli

## Description

Robocode adalah permainan pemrograman di mana pemain membuat kode bot berbentuk tank virtual yang berkompetisi dalam pertempuran hingga tersisa satu bot sebagai pemenang, mirip dengan konsep Battle Royale.

Dalam proyek ini, Cisli membuat 4 bot dengan menggunakan strategi greedy yang dapat digunakan untuk bermain.

## Bots

- CisliMafiaBossLv100: Menggunakan strategi greedy dengan memaksimalkan efisiensi energi melalui tembakan bertenaga penuh pada jarak dekat, menghindari serangan musuh dengan pergerakan acak, dan memprediksi posisi musuh untuk tembakan yang lebih akurat.
- CisliMafiaLv50: Menerapkan strategi greedy dengan mengunci target terdekat, menabrak musuh untuk memberikan damage tambahan, dan menyesuaikan kekuatan tembakan berdasarkan jarak guna mengoptimalkan penggunaan energi.
- CisliCrookLv10: Menggunakan pendekatan greedy dalam memilih kekuatan tembakan yang disesuaikan dengan jarak musuh, serta bergerak secara strategis pada sudut 90 derajat untuk mempersulit musuh dalam menyerang.
- CisliCrookLv1: Menerapkan strategi greedy dengan memprioritaskan musuh terdekat sebagai target utama, menembak dengan kekuatan yang sesuai jarak untuk menghemat energi, dan bergerak secara acak untuk menghindari serangan musuh.

## Requirements

- robocode-tankroyale-gui-0.30.0.jar
- Dotnet versi 6
- Java versi terbaru

## Usage

- Jalankan java -jar robocode-tankroyale-gui-0.30.0.jar pada terminal untuk memulaikan robocode.
- Untuk mengetahui apa saja yang dapat dilakukan dalam aplikasi robocode, silahkan kunjungi tautan berikut https://robocode-dev.github.io/tank-royale/articles/gui.html

## Authors

- Dave Daniell Yanni - 13523003
- Alvin Christopher Santausa - 13523033
- Yonatan Edward Njoto - 13523036
