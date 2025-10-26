// wwwroot/js/theme-manager.js

/**
 * FlowForge 主题管理器
 * 支持多主题切换和系统主题自动跟随
 */
class ThemeManager {
    constructor() {
        this.currentTheme = this.getStoredTheme() || 'light';
        this.mediaQuery = window.matchMedia('(prefers-color-scheme: dark)');
        this.init();
    }

    init() {
        // 应用初始主题
        this.applyTheme(this.currentTheme);

        // 监听系统主题变化
        this.mediaQuery.addEventListener('change', (e) => {
            if (this.currentTheme === 'auto') {
                this.applyTheme('auto');
            }
        });
    }

    /**
     * 切换主题
     * @param {string} theme - 主题名称: light, dark, violet, pink, auto
     */
    setTheme(theme) {
        this.currentTheme = theme;
        this.applyTheme(theme);
        this.storeTheme(theme);

        // 触发自定义事件
        window.dispatchEvent(new CustomEvent('themeChanged', {
            detail: { theme }
        }));
    }

    /**
     * 应用主题
     */
    applyTheme(theme) {
        const root = document.documentElement;

        if (theme === 'auto') {
            const systemTheme = this.mediaQuery.matches ? 'dark' : 'light';
            root.setAttribute('data-theme', systemTheme);
        } else {
            root.setAttribute('data-theme', theme);
        }

        // 添加过渡动画
        root.style.transition = 'background-color 0.3s ease, color 0.3s ease';
    }

    /**
     * 获取当前主题
     */
    getTheme() {
        return this.currentTheme;
    }

    /**
     * 获取实际应用的主题（处理 auto 情况）
     */
    getAppliedTheme() {
        if (this.currentTheme === 'auto') {
            return this.mediaQuery.matches ? 'dark' : 'light';
        }
        return this.currentTheme;
    }

    /**
     * 存储主题到 localStorage
     */
    storeTheme(theme) {
        localStorage.setItem('flowforge-theme', theme);
    }

    /**
     * 从 localStorage 读取主题
     */
    getStoredTheme() {
        return localStorage.getItem('flowforge-theme');
    }

    /**
     * 切换明暗主题
     */
    toggleTheme() {
        const currentApplied = this.getAppliedTheme();
        const newTheme = currentApplied === 'light' ? 'dark' : 'light';
        this.setTheme(newTheme);
    }
}

// 导出为全局实例
window.themeManager = new ThemeManager();

// 提供给 Blazor 调用的接口
window.ThemeInterop = {
    setTheme: (theme) => window.themeManager.setTheme(theme),
    getTheme: () => window.themeManager.getTheme(),
    toggleTheme: () => window.themeManager.toggleTheme()
};