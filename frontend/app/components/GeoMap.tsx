'use client';
import React from 'react';

const GeoMap = ({ lon, lat }: { lon: number; lat: number }) => {
  const apiKey = process.env.GEOAPIFY_API;
  if (!apiKey || !lon || !lat) return null;

  const mapUrl = `https://maps.geoapify.com/v1/staticmap?style=osm-bright&width=600&height=400&center=lonlat:${lon},${lat}&zoom=15&marker=lonlat:${lon},${lat};color:%23ff0000;size:medium&apiKey=${apiKey}`;

  return (
    <div className="my-4">
      <img src={mapUrl} alt="Map location" width={600} height={400} className="rounded shadow" />
    </div>
  );
};

export default GeoMap;
