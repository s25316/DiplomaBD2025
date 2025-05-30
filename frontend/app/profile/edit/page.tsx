'use client';
import React, { useEffect, useState } from 'react';
import { useSession } from 'next-auth/react';
import RegularProfileForm from '@/app/components/forms/RegularProfileForm';
import { useRouter } from 'next/navigation';

const EditProfilePage = () => {
  const { data: session } = useSession();
  const [userData, setUserData] = useState<any | null>(null);
  const [loading, setLoading] = useState(true);
  const router = useRouter();

  useEffect(() => {
    const fetchUser = async () => {
      if (!session?.user?.token) return;
      const res = await fetch('http://localhost:8080/api/User', {
        headers: {
          Authorization: `Bearer ${session.user.token}`,
        },
      });
      const data = await res.json();
      setUserData(data.personPerspective);
      setLoading(false);
    };

    fetchUser();
  }, [session]);

  if (!session?.user?.token) return <p>Unauthorized</p>;
  if (loading) return <p>Loading...</p>;
  if (!userData) return <p>No data found.</p>;

  return (
    <div className="p-4">
      <h1 className="text-2xl font-semibold mb-4">Edit Profile</h1>
      <RegularProfileForm token={session.user.token} initialData={userData} />
    </div>
  );
};

export default EditProfilePage;