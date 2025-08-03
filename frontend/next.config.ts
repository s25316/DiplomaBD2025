import type { NextConfig } from "next";

const nextConfig: NextConfig = {
  /* config options here */
  env:{
    GEOAPIFY_API: process.env.GEOAPIFY_API,
  },
  async redirects() {
    return [
      {
        source: '/',
        destination: '/offers',
        permanent: true,
      },
    ]
  },
  images: {
    remotePatterns: [{
      protocol: 'https',
      hostname: 'maps.geoapify.com'
    }], // add the map image domain here
  },
  output: "standalone",
};

export default nextConfig;
