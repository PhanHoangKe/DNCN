/**
 * Theme Toggle Functionality
 * Handles dark/light theme switching with localStorage persistence
 */

class ThemeManager {
    constructor() {
        this.themeToggle = null;
        this.themeIcon = null;
        this.html = document.documentElement;
        this.currentTheme = 'light';
        
        this.init();
    }

    init() {
        // Apply theme immediately to prevent flash
        this.currentTheme = localStorage.getItem('theme') || 'light';
        this.html.setAttribute('data-theme', this.currentTheme);
        
        // Wait for DOM to be ready for interactive elements
        if (document.readyState === 'loading') {
            document.addEventListener('DOMContentLoaded', () => this.setupTheme());
        } else {
            this.setupTheme();
        }
    }

    setupTheme() {
        // Get elements
        this.themeToggle = document.getElementById('theme-toggle');
        this.themeIcon = document.getElementById('theme-icon');
        
        if (!this.themeToggle || !this.themeIcon) {
            console.warn('Theme toggle elements not found');
            return;
        }

        // Ensure theme is applied (in case it wasn't applied in init)
        this.currentTheme = localStorage.getItem('theme') || 'light';
        this.html.setAttribute('data-theme', this.currentTheme);
        
        // Update icon to match current theme
        this.updateThemeIcon(this.currentTheme);
        
        // Add click event listener
        this.themeToggle.addEventListener('click', () => this.toggleTheme());
        
        // Listen for storage changes (if user changes theme in another tab)
        window.addEventListener('storage', (e) => {
            if (e.key === 'theme') {
                this.currentTheme = e.newValue || 'light';
                this.applyTheme(this.currentTheme);
            }
        });
    }

    toggleTheme() {
        this.currentTheme = this.currentTheme === 'light' ? 'dark' : 'light';
        this.applyTheme(this.currentTheme);
        this.saveTheme();
    }

    applyTheme(theme) {
        // Apply theme to html element
        this.html.setAttribute('data-theme', theme);
        
        // Update icon and tooltip
        this.updateThemeIcon(theme);
        
        // Dispatch custom event for other scripts to listen to
        window.dispatchEvent(new CustomEvent('themeChanged', {
            detail: { theme: theme }
        }));
    }

    updateThemeIcon(theme) {
        if (!this.themeIcon || !this.themeToggle) return;
        
        if (theme === 'dark') {
            this.themeIcon.className = 'bi bi-sun';
            this.themeToggle.title = 'Switch to Light Theme';
        } else {
            this.themeIcon.className = 'bi bi-moon';
            this.themeToggle.title = 'Switch to Dark Theme';
        }
    }

    saveTheme() {
        try {
            localStorage.setItem('theme', this.currentTheme);
        } catch (error) {
            console.warn('Could not save theme to localStorage:', error);
        }
    }

    getCurrentTheme() {
        return this.currentTheme;
    }

    setTheme(theme) {
        if (theme === 'light' || theme === 'dark') {
            this.currentTheme = theme;
            this.applyTheme(theme);
            this.saveTheme();
        }
    }
}

// Initialize theme manager immediately to prevent flash
(function() {
    const savedTheme = localStorage.getItem('theme') || 'light';
    document.documentElement.setAttribute('data-theme', savedTheme);
})();

// Initialize theme manager when DOM is ready
let themeManager;

// Initialize immediately if DOM is already loaded, otherwise wait for DOMContentLoaded
if (document.readyState === 'loading') {
    document.addEventListener('DOMContentLoaded', () => {
        themeManager = new ThemeManager();
    });
} else {
    // DOM is already loaded, initialize immediately
    themeManager = new ThemeManager();
}

// Export for global access
window.ThemeManager = ThemeManager;
window.themeManager = themeManager;

// Utility functions for other scripts
window.getCurrentTheme = () => themeManager ? themeManager.getCurrentTheme() : 'light';
window.setTheme = (theme) => themeManager ? themeManager.setTheme(theme) : null;
window.toggleTheme = () => themeManager ? themeManager.toggleTheme() : null;