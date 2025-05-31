'use client';
import React, { useEffect, useState } from 'react';
import { useRouter } from 'next/navigation';
import { GeocoderAutocomplete } from '@geoapify/geocoder-autocomplete';
import '@geoapify/geocoder-autocomplete/styles/round-borders.css';


interface Props {
  initialData: any;
  token: string;
}

const RegularProfileForm = ({ initialData, token }: Props) => {
  const [skills, setSkills] = useState([]);
  const [urlTypes, setUrlTypes] = useState([]);
  const router = useRouter();

  const [address, setAddress] = useState({
    countryName: initialData.address?.countryName || '',
    stateName: initialData.address?.stateName || '',
    cityName: initialData.address?.cityName || '',
    streetName: initialData.address?.streetName || '',
    houseNumber: initialData.address?.houseNumber || '',
    apartmentNumber: initialData.address?.apartmentNumber || '',
    postCode: initialData.address?.postCode || '',
    lon: initialData.address?.lon || 0,
    lat: initialData.address?.lat || 0,
  });

  const [form, setForm] = useState({
    description: initialData.description || '',
    contactEmail: initialData.contactEmail || '',
    contactPhoneNumber: initialData.phoneNum || '',
    birthDate: initialData.birthDate?.substring(0, 10) || '',
    isTwoFactorAuthentication: initialData.isTwoFactorAuth || false,
    isStudent: initialData.isStudent || false,
    skillsIds: initialData.skills?.map((s: any) => s.skillId) || [],
    urls: initialData.urls?.map((u: any) => ({ value: u.value, urlTypeId: u.urlType.urlTypeId })) || [],
  });

  useEffect(() => {
    const headers = { Authorization: `Bearer ${token}` };
    Promise.all([
      fetch('http://localhost:8080/api/Dictionaries/skills', { headers }),
      fetch('http://localhost:8080/api/Dictionaries/urlTypes', { headers }),
    ]).then(async ([skillsRes, urlTypesRes]) => {
      setSkills(await skillsRes.json());
      setUrlTypes(await urlTypesRes.json());
    });

    const apiKey = process.env.GEOAPIFY_API!;
    const container = document.getElementById('autocomplete-container');

    if (container && !container.children.length) {
      const geo = new GeocoderAutocomplete(container, apiKey, {
        lang: 'en',
        filter: { countrycode: ['pl'] },
      });

      geo.on('select', (location) => {
        const props = location.properties;
        setAddress({
          countryName: props.country || '',
          stateName: props.state || '',
          cityName: props.city || '',
          streetName: props.street || '',
          houseNumber: props.housenumber || '',
          apartmentNumber: '',
          postCode: props.postcode || '',
          lon: props.lon,
          lat: props.lat,
        });
      });
    }
  }, []);

  const updateUrl = (index: number, field: 'value' | 'urlTypeId', value: any) => {
    const updated = [...form.urls];
    updated[index] = { ...updated[index], [field]: value };
    setForm({ ...form, urls: updated });
  };

  const handleSubmit = async () => {
    const deduplicatedUrls = form.urls
      .filter((u : { value: string }) => u.value.trim() !== '')
      .filter((value: { value: string; urlTypeId: number }, index: number, self: { value: string; urlTypeId: number }[]) =>
        index === self.findIndex((v) => v.value === value.value && v.urlTypeId === value.urlTypeId)
      );

    const fullPayload = {
      ...form,
      urls: deduplicatedUrls,
      address,
    };

    const res = await fetch('http://localhost:8080/api/User/regularData', {
      method: 'PUT',
      headers: {
        'Content-Type': 'application/json',
        Authorization: `Bearer ${token}`,
      },
      body: JSON.stringify(fullPayload),
    });


    if (res.ok){
      alert('Profile updated!');
      router.push('/profile');
    }
    else alert('Failed to update profile');
  };

  const removeUrl = (index: number) => {
    setForm((prevForm) => ({
      ...prevForm,
      urls: prevForm.urls.filter((_: any, i: number) => i !== index),
    }));
  };


  return (
    <div className="flex flex-col gap-4 mt-4 max-w-2xl">
      <label><b>Description</b></label>
      <textarea value={form.description} onChange={(e) => setForm({ ...form, description: e.target.value })} />

      <label><b>Contact Email</b></label>
      <input value={form.contactEmail} onChange={(e) => setForm({ ...form, contactEmail: e.target.value })} />

      <label><b>Phone Number</b></label>
      <input value={form.contactPhoneNumber} onChange={(e) => setForm({ ...form, contactPhoneNumber: e.target.value })} />

      <label><b>Birth Date</b></label>
      <input type="date" value={form.birthDate} onChange={(e) => setForm({ ...form, birthDate: e.target.value })} />
      {(address.countryName != "") && (
        <>
      <label><b>Current Address</b></label>
      <p>
        {[
          "'ul. ",
          address.streetName, " ",
          address.houseNumber, "/ ",
          address.apartmentNumber,", ",
          address.postCode, ", ",
          address.cityName, " ",
          address.countryName,"'"
        ]
          .filter(Boolean)
          .join('')}
      </p>
      </>
      )}

     <p className="text-gray-700 text-sm italic">Address (search or edit)</p>
      <div id="autocomplete-container" style={{ position: 'relative' }} />
      <label>Apartment Number</label>
      <input
        type="text"
        value={address.apartmentNumber || ''}
        onChange={(e) => setAddress({ ...address, apartmentNumber: e.target.value })}
      />
      
      <label>
        <input
          type="checkbox"
          checked={form.isTwoFactorAuthentication}
          onChange={(e) => setForm({ ...form, isTwoFactorAuthentication: e.target.checked })}
        /> Two-Factor Authentication
      </label>

      <label>
        <input
          type="checkbox"
          checked={form.isStudent}
          onChange={(e) => setForm({ ...form, isStudent: e.target.checked })}
        /> Are you a student?
      </label>

      <label><b>Skills</b></label>
      <div className="grid grid-cols-2 gap-1">
        {skills.map((skill: any) => (
          <label key={skill.skillId} className="text-sm">
            <input
              type="checkbox"
              checked={form.skillsIds.includes(skill.skillId)}
              onChange={(e) => {
                const selected = e.target.checked;
                setForm((prev) => ({
                  ...prev,
                  skillsIds: selected
                    ? [...prev.skillsIds, skill.skillId]
                    : prev.skillsIds.filter((id : {value : number}) => id !== skill.skillId),
                }));
              }}
            />
            {skill.name}
          </label>
        ))}
      </div>

      <label>Links</label>
      {form.urls.map((url: { value: string; urlTypeId: number }, index: number) => (
        <div key={index} className="flex gap-2 items-center">
          <input
            type="text"
            value={url.value}
            placeholder="https://..."
            onChange={(e) => updateUrl(index, 'value', e.target.value)}
            className="flex-1"
          />
          <select
            value={url.urlTypeId}
            onChange={(e) => updateUrl(index, 'urlTypeId', Number(e.target.value))}
          >
            {urlTypes.map((t: any) => (
              <option key={t.urlTypeId} value={t.urlTypeId}>{t.name}</option>
            ))}
          </select>
          <button
            type="button"
            onClick={() => removeUrl(index)}
            className="text-red-500 hover:underline"
            title="Remove link" > Remove
          </button>
        </div>
      ))}
      <button type="button" onClick={() => setForm({ ...form, urls: [...form.urls, { value: '', urlTypeId: 1 }] })}>
        Add Link
      </button>

      <button
        onClick={handleSubmit}
        className="bg-blue-600 text-white py-2 px-4 rounded mt-4" > Update Profile
      </button>
    </div>
  );
};

export default RegularProfileForm;
