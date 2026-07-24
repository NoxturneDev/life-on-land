import os
import matplotlib.pyplot as plt
import matplotlib.patches as patches
import numpy as np

out_dir = r"c:\Users\galih\Documents\Projects\Game\My project\docs\submissions\charts"
os.makedirs(out_dir, exist_ok=True)

# Google Forms Colors
G_BLUE = "#4285f4"
G_RED = "#ea4335"
G_YELLOW = "#fbbc04"
G_GREEN = "#34a853"
G_PURPLE = "#a142f4"
G_TEAL = "#24c1e0"
G_ORANGE = "#ff6d00"

GOOGLE_COLORS = [G_BLUE, G_RED, G_YELLOW, G_GREEN, G_PURPLE, G_TEAL, G_ORANGE]
BG_PURPLE = "#f0ebf8"
CARD_BG = "#ffffff"
BORDER_COLOR = "#dadce0"
TEXT_DARK = "#202124"
TEXT_SUB = "#70757a"

plt.rcParams['font.sans-serif'] = ['Segoe UI', 'Roboto', 'Arial', 'sans-serif']

def create_gform_card_pie(title, responses_count_str, labels, values, filename):
    fig = plt.figure(figsize=(9, 4.8), dpi=200, facecolor=BG_PURPLE)
    
    # Background axis for card frame
    ax_bg = fig.add_axes([0, 0, 1, 1], zorder=0)
    ax_bg.set_facecolor(BG_PURPLE)
    ax_bg.axis('off')
    
    # White card with rounded corners
    card = patches.FancyBboxPatch((0.03, 0.04), 0.94, 0.92,
                                 boxstyle="round,pad=0.015,rounding_size=0.03",
                                 facecolor=CARD_BG, edgecolor=BORDER_COLOR, linewidth=1.2,
                                 zorder=1, transform=ax_bg.transAxes)
    ax_bg.add_patch(card)
    
    # Title & Subtitle
    ax_bg.text(0.07, 0.86, title, fontsize=13.5, fontweight='bold', color=TEXT_DARK, va='top', zorder=2)
    ax_bg.text(0.07, 0.77, responses_count_str, fontsize=9.5, color=TEXT_SUB, va='top', zorder=2)
    
    # Subplot for Pie Chart
    ax_pie = fig.add_axes([0.06, 0.08, 0.44, 0.62], zorder=10)
    colors = GOOGLE_COLORS[:len(labels)]
    
    wedges, texts, autotexts = ax_pie.pie(
        values, 
        colors=colors, 
        autopct='%1.1f%%', 
        startangle=140,
        pctdistance=0.62,
        wedgeprops=dict(width=1.0, edgecolor='white', linewidth=1.8)
    )
    
    for at in autotexts:
        at.set_color('white')
        at.set_fontsize(9)
        at.set_weight('bold')
        
    ax_pie.axis('equal')
    
    # Legend Axis on the right
    ax_legend = fig.add_axes([0.52, 0.08, 0.43, 0.64], zorder=10)
    ax_legend.axis('off')
    
    total = sum(values)
    y_pos = 0.85
    step = 0.80 / max(len(labels), 1)
    
    for i, (label, val, col) in enumerate(zip(labels, values, colors)):
        pct = (val / total) * 100
        # Color circle
        circle = plt.Circle((0.04, y_pos), 0.028, color=col, transform=ax_legend.transAxes)
        ax_legend.add_patch(circle)
        # Label & value text
        ax_legend.text(0.09, y_pos, f"{label}", fontsize=9.5, color=TEXT_DARK, va='center', transform=ax_legend.transAxes)
        ax_legend.text(0.09, y_pos - 0.08, f"{val} ({pct:.1f}%)", fontsize=8.5, color=TEXT_SUB, va='center', transform=ax_legend.transAxes)
        y_pos -= step

    plt.savefig(os.path.join(out_dir, filename), facecolor=fig.get_facecolor(), bbox_inches='tight')
    plt.close()
    print(f"Generated {filename}")

def create_gform_card_bar(title, responses_count_str, labels, values, max_val, filename):
    fig = plt.figure(figsize=(9, 5.0), dpi=200, facecolor=BG_PURPLE)
    
    ax_bg = fig.add_axes([0, 0, 1, 1], zorder=0)
    ax_bg.set_facecolor(BG_PURPLE)
    ax_bg.axis('off')
    
    card = patches.FancyBboxPatch((0.03, 0.04), 0.94, 0.92,
                                 boxstyle="round,pad=0.015,rounding_size=0.03",
                                 facecolor=CARD_BG, edgecolor=BORDER_COLOR, linewidth=1.2,
                                 zorder=1, transform=ax_bg.transAxes)
    ax_bg.add_patch(card)
    
    ax_bg.text(0.07, 0.87, title, fontsize=13.5, fontweight='bold', color=TEXT_DARK, va='top', zorder=2)
    ax_bg.text(0.07, 0.78, responses_count_str, fontsize=9.5, color=TEXT_SUB, va='top', zorder=2)
    
    # Bar Subplot
    ax_bar = fig.add_axes([0.32, 0.12, 0.62, 0.58], zorder=10)
    ax_bar.set_facecolor(CARD_BG)
    
    y_pos = np.arange(len(labels))
    bars = ax_bar.barh(y_pos, values, align='center', color=G_BLUE, height=0.55, edgecolor='none')
    
    ax_bar.set_yticks(y_pos)
    ax_bar.set_yticklabels(labels, fontsize=9.5, color=TEXT_DARK)
    ax_bar.invert_yaxis()
    ax_bar.set_xlim(0, max_val * 1.15)
    
    for spine in ['top', 'right', 'bottom', 'left']:
        ax_bar.spines[spine].set_color(BORDER_COLOR if spine=='left' else 'none')
        
    ax_bar.tick_params(axis='x', colors=TEXT_SUB, labelsize=8.5)
    
    for bar in bars:
        width = bar.get_width()
        ax_bar.text(width + (max_val*0.02), bar.get_y() + bar.get_height()/2, f'{width:g}',
                    va='center', ha='left', fontsize=9, fontweight='bold', color=TEXT_DARK)

    plt.savefig(os.path.join(out_dir, filename), facecolor=fig.get_facecolor(), bbox_inches='tight')
    plt.close()
    print(f"Generated {filename}")

def create_gform_card_aspect_bars(title, responses_count_str, labels, scores_pct, filename):
    fig = plt.figure(figsize=(9, 5.2), dpi=200, facecolor=BG_PURPLE)
    
    ax_bg = fig.add_axes([0, 0, 1, 1], zorder=0)
    ax_bg.set_facecolor(BG_PURPLE)
    ax_bg.axis('off')
    
    card = patches.FancyBboxPatch((0.03, 0.04), 0.94, 0.92,
                                 boxstyle="round,pad=0.015,rounding_size=0.03",
                                 facecolor=CARD_BG, edgecolor=BORDER_COLOR, linewidth=1.2,
                                 zorder=1, transform=ax_bg.transAxes)
    ax_bg.add_patch(card)
    
    ax_bg.text(0.07, 0.88, title, fontsize=13.5, fontweight='bold', color=TEXT_DARK, va='top', zorder=2)
    ax_bg.text(0.07, 0.79, responses_count_str, fontsize=9.5, color=TEXT_SUB, va='top', zorder=2)
    
    ax_bar = fig.add_axes([0.08, 0.14, 0.86, 0.56], zorder=10)
    ax_bar.set_facecolor(CARD_BG)
    
    x_pos = np.arange(len(labels))
    bars = ax_bar.bar(x_pos, scores_pct, color=G_BLUE, width=0.42, edgecolor='none')
    
    ax_bar.set_xticks(x_pos)
    ax_bar.set_xticklabels(labels, fontsize=8.5, color=TEXT_DARK, multialignment='center')
    ax_bar.set_ylim(0, 100)
    
    for spine in ['top', 'right', 'left']:
        ax_bar.spines[spine].set_visible(False)
    ax_bar.spines['bottom'].set_color(BORDER_COLOR)
    ax_bar.tick_params(axis='y', colors=TEXT_SUB, labelsize=8.5)
    
    for bar in bars:
        height = bar.get_height()
        ax_bar.text(bar.get_x() + bar.get_width()/2., height + 2,
                    f'{height:.1f}%',
                    ha='center', va='bottom', fontsize=9, fontweight='bold', color=TEXT_DARK)

    plt.savefig(os.path.join(out_dir, filename), facecolor=fig.get_facecolor(), bbox_inches='tight')
    plt.close()
    print(f"Generated {filename}")

# Generate All Charts

# 1. Demografi Jenis Kelamin
create_gform_card_pie(
    title="Jenis Kelamin Responden",
    responses_count_str="21 tanggapan",
    labels=["Laki-laki", "Perempuan"],
    values=[11, 10],
    filename="gform_chart_1_jenis_kelamin.png"
)

# 2. Demografi Usia
create_gform_card_pie(
    title="Usia Responden",
    responses_count_str="21 tanggapan",
    labels=["18 - 22 Tahun", "23 - 25 Tahun", "26 - 30 Tahun", "> 30 Tahun"],
    values=[11, 6, 1, 3],
    filename="gform_chart_2_usia.png"
)

# 3. Demografi Pekerjaan
create_gform_card_pie(
    title="Pekerjaan Responden",
    responses_count_str="21 tanggapan",
    labels=["Pelajar / Mahasiswa", "Karyawan / Pegawai", "Lainnya"],
    values=[14, 3, 4],
    filename="gform_chart_3_pekerjaan.png"
)

# 4. Keakraban dengan Game Eco-Sim / Gamifikasi
create_gform_card_pie(
    title="Tingkat Keakraban dengan Game Eco-Sim / Gamifikasi",
    responses_count_str="21 tanggapan",
    labels=["Tahu konsep dasarnya saja", "Cukup paham", "Sangat paham"],
    values=[11, 7, 3],
    filename="gform_chart_4_familiaritas.png"
)

# 5. Rating Usability SUS Alpha (Score Distribution)
create_gform_card_bar(
    title="Pengujian Alpha — Distribusi Skor SUS (Skala Likert 1-4)",
    responses_count_str="21 tanggapan (Rata-rata Skor SUS: 63.45 / Grade D - OK)",
    labels=["Grade A (80-100)", "Grade B (70-79)", "Grade D (55-69)", "Grade F (< 55)"],
    values=[4, 1, 11, 5],
    max_val=12,
    filename="gform_chart_5_sus_scores.png"
)

# 6. Aspek Penilaian UAT Beta (Persentase Keberhasilan)
create_gform_card_aspect_bars(
    title="Pengujian Beta — Persentase Keberhasilan Aspek UAT",
    responses_count_str="21 tanggapan (Skala Likert 1-5)",
    labels=["UI Aesthetics\n(Tampilan UI)", "Intuitiveness\n(Navigasi Menu)", "Functional\n(Fungsional Fitur)", "Responsiveness\n(Performa/Kecepatan)", "Suitability\n(Kesesuaian Game)"],
    scores_pct=[84.8, 83.8, 81.0, 80.0, 82.8],
    filename="gform_chart_6_uat_aspects.png"
)

# 7. Overall UAT Success Rating
create_gform_card_pie(
    title="Hasil Akhir Evaluasi User Acceptance Testing (UAT)",
    responses_count_str="21 tanggapan (Skor Rata-rata: 4.12 / 5.00 - 82.4%)",
    labels=["Sangat Layak / Sangat Berhasil (>= 80%)", "Layak / Cukup Berhasil (60% - 79%)"],
    values=[18, 3],
    filename="gform_chart_7_uat_overall.png"
)

print("\nALL GOOGLE FORM CHARTS FIXED & GENERATED SUCCESSFULLY!")
