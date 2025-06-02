import { Component, OnInit, AfterViewInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { NavbarComponent } from '../navbar/navbar.component';
import * as L from 'leaflet';

@Component({
  selector: 'app-customer-dashboard',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './customer-dashboard.component.html',
  styleUrls: ['./customer-dashboard.component.css']
})
export class CustomerDashboardComponent implements OnInit, AfterViewInit, OnDestroy {
  results: any[] = [];
  lat: number | null = null;
  lon: number | null = null;
  isLoading: boolean = false;
  errorMessage: string | null = null;
  private map!: L.Map;
  private markers: L.Marker[] = [];

  private getRedIcon(): L.Icon {
    return L.icon({
      iconUrl: 'assets/gps.png',
      iconSize: [25, 41],
      iconAnchor: [12, 41],
      popupAnchor: [1, -34],
    });
  }

  private getBlueIcon(): L.Icon {
    return L.icon({
      iconUrl: 'assets/location-pin.png',
      iconSize: [25, 41],
      iconAnchor: [12, 41],
      popupAnchor: [1, -34],
    });
  }

  predefinedLocations = [
    { name: 'Trivandrum', lat: 8.4871538, lon: 76.9476994 },
    { name: 'Bangalore', lat: 12.9767936, lon: 77.590082 },
    { name: 'Kochi', lat: 9.9674277, lon: 76.2454436 },
    { name: 'Coorg', lat: 12.4225809, lon: 75.7365857 }
  ];

  predefinedCategories = [
    { code: 'commercial.food_and_drink', label: 'Restaurant' },
    { code: 'commercial.garden', label: 'Garden' },
    { code: 'accommodation.hotel', label: 'Hotel' },
    { code: 'accommodation.guest_house', label: 'Guest House' },
    { code: 'beach.beach_resort', label: 'Beach Resort' }
  ];

  selectedCategory: string = '';

  constructor(private http: HttpClient) {}

  ngOnInit(): void {}

  ngAfterViewInit(): void {
    setTimeout(() => {
      this.initMap();
    }, 0);
  }

  ngOnDestroy(): void {
    if (this.map) {
      this.map.remove();
    }
  }

  private initMap(): void {
    // Default to Trivandrum if no location selected
    const defaultLat = 8.4871538;
    const defaultLon = 76.9476994;
  
    // Check if map already exists
    if (this.map) {
      this.map.remove();
    }
  
    // Create new map
    this.map = L.map('map', {
      center: [defaultLat, defaultLon],
      zoom: 14,
      renderer: L.canvas()
    });
  
    // Add tile layer
    L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
      attribution: '&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors',
      maxZoom: 19,
    }).addTo(this.map);
  
    // Force a redraw
    setTimeout(() => {
      this.map.invalidateSize();
    }, 0);


    console.log('Leaflet available:', L); // Should show Leaflet object
    console.log('Map container exists:', document.getElementById('map')); // Should show div element
  
  }

  onLocationChange(event: Event): void {
    const value = (event.target as HTMLSelectElement).value;
    const [latStr, lonStr] = value.split(',');
    this.lat = parseFloat(latStr);
    this.lon = parseFloat(lonStr);
    
    // Clear existing markers
    this.clearMarkers();

    // Add marker for selected location
    if (this.lat && this.lon) {
      const marker = L.marker([this.lat, this.lon], {
        icon: this.getDefaultIcon()
      }).bindPopup('Selected Location')
        .addTo(this.map);
      
      this.markers.push(marker);
      this.map.setView([this.lat, this.lon], 14);
    }
  }

  searchPlaces(): void {
    if (!this.lat || !this.lon || !this.selectedCategory) {
      this.errorMessage = 'Please select both location and category';
      return;
    }
  
    this.isLoading = true;
    this.errorMessage = null;
    this.results = [];
    this.clearMarkers();

    const requestData = {
      latitude: this.lat,
      longitude: this.lon,
      category: this.selectedCategory
    };
    
    this.http.post<any>('https://localhost:5009/api/GeoSearch/search-location', requestData).subscribe({
      next: (response) => {
        if (response.result && response.result.features && response.result.features.length > 0) {
          this.results = response.result.features;
          this.updateMapWithResults(response.result.features);
        } else {
          this.results = [];
          this.errorMessage = 'No places found for this category in the selected location';
        }
        this.isLoading = false;
      },
      error: (error) => {
        console.error('API Error:', error);
        this.errorMessage = error.error?.error || 'Error fetching places. Please try again.';
        this.isLoading = false;
      }
    });
  }

  private updateMapWithResults(features: any[]): void {
  this.clearMarkers();

  // Add blue marker for selected location
  if (this.lat && this.lon) {
    const marker = L.marker([this.lat, this.lon], {
      icon: this.getBlueIcon()
    }).bindPopup('Selected Location')
      .addTo(this.map);
    
    this.markers.push(marker);
  }

  // Add red markers for each result
  const featureGroup: L.Layer[] = [];
  features.forEach(feature => {
    const coords = feature.geometry.coordinates;
    const marker = L.marker([coords[1], coords[0]], {
      icon: this.getRedIcon()
    }).bindPopup(`
      <b>${feature.properties.name || 'Unnamed Place'}</b><br>
      ${feature.properties.address_line1}<br>
      ${feature.properties.address_line2}<br>
      Distance: ${feature.properties.distance.toFixed(0)} meters<br>
      <a href="${this.getGoogleMapsDirectionUrl(coords[1], coords[0])}" target="_blank">Get Directions</a>
    `).addTo(this.map);

    this.markers.push(marker);
    featureGroup.push(marker);
  });

  if (featureGroup.length > 0) {
    const group = L.featureGroup(featureGroup);
    this.map.fitBounds(group.getBounds().pad(0.2));
  }
}

// Add this method for Google Maps directions
private getGoogleMapsDirectionUrl(lat: number, lng: number): string {
  return `https://www.google.com/maps/dir/?api=1&destination=${lat},${lng}&travelmode=driving`;
}
openDirections(place: any): void {
  const coords = place.geometry.coordinates;
  const url = this.getGoogleMapsDirectionUrl(coords[1], coords[0]);
  window.open(url, '_blank');
}

  focusOnMarker(place: any): void {
    const coords = place.geometry.coordinates;
    this.map.setView([coords[1], coords[0]], 16);
    
    // Open the popup for this marker
    const marker = this.markers.find(m => {
      const latLng = m.getLatLng();
      return latLng.lat === coords[1] && latLng.lng === coords[0];
    });
    
    if (marker) {
      marker.openPopup();
    }
  }

  private clearMarkers(): void {
    this.markers.forEach(marker => {
      this.map.removeLayer(marker);
    });
    this.markers = [];
  }

  private getDefaultIcon(): L.Icon {
    return L.icon({
      iconUrl: 'assets/marker-icon.png',
      shadowUrl: 'assets/marker-shadow.png',
      iconSize: [25, 41],
      iconAnchor: [12, 41],
      popupAnchor: [1, -34],
      shadowSize: [41, 41]
    });
  }
}