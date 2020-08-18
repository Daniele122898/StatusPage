import {Injectable, Renderer2, RendererFactory2} from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class ColorSchemeService {

  private renderer: Renderer2;

  private colorScheme: string;
  private colorSchemePrefix = 'color-scheme-';

  constructor(rendererFactory: RendererFactory2) {
    this.renderer = rendererFactory.createRenderer(null, null);
  }

  public load(): void {
    this.getColorScheme();
    this.renderer.addClass(document.body, this.colorSchemePrefix + this.colorScheme);
  }

  public update(scheme): void {
    this.setColorScheme(scheme);
    // Remove the old color-scheme class
    this.renderer.removeClass(document.body, this.colorSchemePrefix + (this.colorScheme === 'dark' ? 'light' : 'dark') );
    // Add the new / current color-scheme class
    this.renderer.addClass(document.body, this.colorSchemePrefix + scheme);
  }

  public currentActive(): string {
    return this.colorScheme;
  }

  private detectPrefersColorScheme(): void {
    // Detect if prefers-color-scheme is supported
    if (window.matchMedia('(prefers-color-scheme)').media !== 'not all') {
      // Set colorScheme to Dark if prefers-color-scheme is dark. Otherwise set to light.
      this.colorScheme = window.matchMedia('(prefers-color-scheme: dark)').matches ? 'dark' : 'light';
    } else {
      // If browser dont support prefers-color-scheme, set it as default to dark
      this.colorScheme = 'light';
    }
  }

  private setColorScheme(scheme): void {
    this.colorScheme = scheme;
    // Save prefers-color-scheme to localStorage
    localStorage.setItem('prefers-color', scheme);
  }

  private getColorScheme(): void {
    // Check if any prefers-color-scheme is stored in localStorage
    let scheme = localStorage.getItem('prefers-color');
    if (scheme) {
      // Check if scheme that is saved is no longer supported
      if (scheme !== 'light' && scheme !== 'dark') {
        scheme = 'light';
        localStorage.removeItem('prefers-color');
      }
      this.colorScheme = scheme;
    } else {
      // If no prefers-color-scheme is stored in localStorage, Try to detect OS default prefers-color-scheme
      this.detectPrefersColorScheme();
    }
  }

}
