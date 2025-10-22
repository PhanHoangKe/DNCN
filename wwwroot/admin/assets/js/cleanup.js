/**
 * Cleanup utilities for admin pages
 * This file handles cleanup of resources when navigating between pages
 */

(function() {
  'use strict';

  // Global cleanup registry
  const cleanupRegistry = {
    datatables: [],
    observers: [],
    timers: [],
    eventListeners: []
  };

  // Register cleanup function
  window.registerCleanup = function(type, cleanupFn) {
    if (cleanupRegistry[type]) {
      cleanupRegistry[type].push(cleanupFn);
    }
  };

  // Perform cleanup
  window.performCleanup = function() {
    // Clean up datatables
    cleanupRegistry.datatables.forEach(cleanup => {
      try {
        cleanup();
      } catch (e) {
        console.warn('Error cleaning up datatable:', e);
      }
    });
    cleanupRegistry.datatables = [];

    // Clean up observers
    cleanupRegistry.observers.forEach(cleanup => {
      try {
        cleanup();
      } catch (e) {
        console.warn('Error cleaning up observer:', e);
      }
    });
    cleanupRegistry.observers = [];

    // Clean up timers
    cleanupRegistry.timers.forEach(cleanup => {
      try {
        cleanup();
      } catch (e) {
        console.warn('Error cleaning up timer:', e);
      }
    });
    cleanupRegistry.timers = [];

    // Clean up event listeners
    cleanupRegistry.eventListeners.forEach(cleanup => {
      try {
        cleanup();
      } catch (e) {
        console.warn('Error cleaning up event listener:', e);
      }
    });
    cleanupRegistry.eventListeners = [];
  };

  // Listen for page navigation events
  window.addEventListener('beforeunload', window.performCleanup);
  window.addEventListener('pagehide', window.performCleanup);
  
  // Listen for popstate events (back/forward navigation)
  window.addEventListener('popstate', window.performCleanup);

  // Listen for hashchange events
  window.addEventListener('hashchange', window.performCleanup);

})();