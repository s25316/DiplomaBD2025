'use client';
import React from 'react';
import Image from 'next/image';

const GeoMap = ({ lon, lat }: { lon: number; lat: number }) => {
  const apiKey = process.env.NEXT_PUBLIC_GEOAPIFY_API;
  if (!apiKey || !lon || !lat) {
    console.error("GeoMap: Missing Geoapi key or coordinates.", { apiKey, lon, lat });
    return null;
  }

  const mapUrl = `https://maps.geoapify.com/v1/staticmap?style=osm-bright&width=600&height=400&center=lonlat:${lon},${lat}&zoom=15&marker=lonlat:${lon},${lat};color:%23ff0000;size:medium&apiKey=${apiKey}`;

  return (
    <div className="my-4">
      <Image src={mapUrl} alt="Map location" width={600} height={400} className="rounded shadow" unoptimized />
    </div>
  );
};

export default GeoMap;