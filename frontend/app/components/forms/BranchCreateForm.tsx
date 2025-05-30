"use client"
import React, { useState, useEffect, ReactEventHandler } from 'react'
import { GeocoderAutocomplete } from '@geoapify/geocoder-autocomplete'
import "@geoapify/geocoder-autocomplete/styles/round-borders.css"

interface Props {
  index: number,
  getData: Function,
  initialData?: any; //do edycji
}

const BranchCreateForm = (props: Props) => {
  const [autocomplete, setAutocomplete] = useState<GeocoderAutocomplete>()
  const [form, setForm] = useState({
    index: props.index,
    name: props.initialData?.name ||"",
    description:props.initialData?.description || null,
    address: {
      apartmentNumber:props.initialData?.address?.apartmentNumber || null,
    },
  })
  const [address, setAddress] = useState({
    countryName: props.initialData?.address?.countryName || "",
    stateName: props.initialData?.address?.stateName || "",
    cityName: props.initialData?.address?.cityName || "",
    streetName: props.initialData?.address?.streetName || null,
    houseNumber: props.initialData?.address?.houseNumber || "",
    postCode: props.initialData?.address?.postCode || "",
    lon: props.initialData?.address?.lon || 0,
    lat: props.initialData?.address?.lat || 0,
  })

  useEffect(() => {
    let api = process.env.GEOAPIFY_API;
    let elem = document.getElementById(`autocomplete-${props.index}`);
    if (!autocomplete && elem && api) {
      if (elem.children.length === 0) {
        let tmpAutocomplete = new GeocoderAutocomplete(
          elem,
          api,
          {
            lang: 'en',
            filter: {
              'countrycode': ['pl'],
            },
          },
        )

        tmpAutocomplete.on("select", (location) => {
          setAddress({
            ...address,
            countryName: location.properties.country,
            stateName: location.properties.state,
            cityName: location.properties.city,
            streetName: location.properties.street ? location.properties.street : null,
            houseNumber: location.properties.housenumber,
            postCode: location.properties.postcode,
            lon: location.properties.lon,
            lat: location.properties.lat,
          })
        })

        setAutocomplete(tmpAutocomplete)
      }
    }

    props.getData({...form, address: {
      ...form.address,
      ...address,
    }})
  }, [form, address])

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setForm({ ...form, [e.target.name]: e.target.value })
  }
  const handleAddressChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setForm({ ...form, address: { ...form.address, [e.target.name]: e.target.value } })
  }

  return (
    <div className='flex flex-col gap-4'>
      <label htmlFor='name'>Branch name:</label>
      <input type='text' name='name' placeholder='Branch name' value={form.name} onChange={handleChange} required />
      <label htmlFor='description'>Description:</label>
      <input type='text' name='description' placeholder='Description' value={form.description ?? ""} onChange={handleChange} />
      <label htmlFor='address'>Address:</label>
      <div id={`autocomplete-${props.index}`} className='autocomplete-container' style={{ position: 'relative' }}></div>
      <label htmlFor='apartmentNumber'>Apartment number:</label>
      <input type='text' name='apartmentNumber' placeholder='123' value={form.address.apartmentNumber ?? ""} onChange={handleAddressChange} />
    </div>
  )
}

export default BranchCreateForm